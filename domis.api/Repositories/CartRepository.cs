using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Image;
using domis.api.DTOs.Order;
using domis.api.DTOs.Product;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Serilog;

namespace domis.api.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses();
    Task<int> CreateCartAsync(string? userId);
    Task<bool> UpdateCartStatusAsync(int cartId, int statusId);
    Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal quantity, string? userId, decimal discount);
    Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal quantity);
    Task<bool> DeleteCartItemAsync(int cartItemId);
    Task<bool> DeleteCartAsync(int cartId);
    Task<CartDto?> GetCart(string? userId, int? cartId);
    Task<bool> SetCartUserId(int cartId, string userId);
}
public class CartRepository(IDbConnection connection) : ICartRepository
{
    public async Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses()
    {
        try
        {
            var orderStatuses = 
                (await connection.QueryAsync<OrderStatusDto>(CartQueries.GetAllCartStatuses))
                .ToList();

            return orderStatuses.Any() ?  orderStatuses: null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching order statuses: {ex.Message}");
            throw;
        }
    }
    
    public async Task<int> CreateCartAsync(string? userId)
    {
        try
        {
            var parameters = new
            {
                UserId = userId,
                StatusId = 1, // Assuming 1 is the status for "Active"
                CreatedAt = DateTime.UtcNow
            };

            var newCartId = await connection.ExecuteScalarAsync<int>(CartQueries.CreateCart, parameters);

            return newCartId;
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while creating a new cart: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> UpdateCartStatusAsync(int cartId, int statusId)
    {
        try
        {
            var parameters = new
            {
                CartId = cartId,
                StatusId = statusId
            };

            int rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartStatus, parameters);
            
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating the cart status: {ex.Message}");
            throw;
        }
    }
    
    public async Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal quantity, string? userId, decimal discount = 0)
    {
        try
        {
            var cartExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfCartExists, new { CartId = cartId });

            //if cart does not exist -> create new one
            if (!cartExists)
            {
                cartId = await CreateCartAsync(userId);
            }

            var productExists = await connection.ExecuteScalarAsync<bool>(ProductQueries.CheckIfProductExists, new { ProductId = productId });
            if (!productExists) throw new NotFoundException($"Product with ID {productId} does not exist.");

            // Check if the product exists in the cart
            var cartItemExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfProductExistsInCart, new { CartId = cartId, ProductId = productId });

            if (cartItemExists)
            {
                var currentQuantity = await connection.ExecuteScalarAsync<decimal>(CartQueries.GetCartItemQuantity, new { CartId = cartId, ProductId = productId });
                // Update the quantity of the existing cart item
                var updateParameters = new
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = currentQuantity + quantity,
                    ModifiedAt = DateTime.UtcNow
                };

                var updatedCartItemId = await connection.ExecuteScalarAsync<int>(CartQueries.UpdateQuantityBasedOnCartAndProduct, updateParameters);
                return cartId;
            }

            // Insert a new cart item
            var price = PricingHelper.CalculateDiscount(
                await connection.ExecuteScalarAsync<decimal>(ProductQueries.GetProductPrice, new { ProductId = productId }),
                discount);

            var parameters = new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,
                Price = price,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            var newCartItemId = await connection.ExecuteScalarAsync<int>(CartQueries.CreateCartItem, parameters);
            return cartId;
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while creating a new cart item: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal quantity)
    {
        try
        {
            var parameters = new
            {
                CartItemId = cartItemId,
                Quantity = quantity,
                ModifiedAt = DateTime.UtcNow
            };
            
            int rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartItemQuantity, parameters);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating the cart item quantity: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> DeleteCartItemAsync(int cartItemId)
    {
        try
        {
            var parameters = new
            {
                CartItemId = cartItemId
            };

            int rowsAffected = await connection.ExecuteAsync(CartQueries.DeleteCartItem, parameters);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while deleting the cart item: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteCartAsync(int cartId)
    {
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var parameters = new { CartId = cartId };

            await connection.ExecuteAsync(CartQueries.DeleteCartItemsQuery, parameters, transaction);
            int rowsAffected = await connection.ExecuteAsync(CartQueries.DeleteCartQuery, parameters, transaction);

            transaction.Commit();

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            Log.Error(ex, $"An error occurred while deleting the cart: {ex.Message}");
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
    
    public Task<bool> SetCartUserId(int cartId, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartDto?> GetCart(string? userId = null, int? cartId = null)
    {
        try
        {
            var cartDictionary = new Dictionary<int, CartDto>();

            string query;
            object parameters;

            if (!string.IsNullOrEmpty(userId))
            {
                query = CartQueries.GetCartByUserId;
                parameters = new { UserId = userId };
            }
            else
            {
                query = CartQueries.GetCartById;
                parameters = new { CartId = cartId };
            }

            var result = await connection.QueryAsync<CartDto, CartItemDto, ProductCartDetailsDto, string, string, CartDto>(
            query,
            (cart, item, product, image, status) =>
            {
                if (!cartDictionary.TryGetValue(cart.CartId, out var currentCart))
                {
                    currentCart = cart;
                    currentCart.Items = new List<CartItemDto>(); // Initialize the Items list properly
                    cartDictionary.Add(currentCart.CartId, currentCart);
                }

                if (item is not null
                    && currentCart.Items.Find(i => i.CartItemId == item.CartItemId) == null)
                {
                    item.ProductDetails = product;
                    item.ProductDetails.Image = image;
                    item.CartItemPrice = item.CartItemPrice;
                    //item.ProductDetails.Price = PricingHelper.CalculateDiscount(item.ProductDetails.Price, discount);
                    currentCart.Items.Add(item);
                }

                currentCart.Status = status; // Assign the status to the cart

                return currentCart;
            },
            parameters,
            splitOn: "CartItemId, Name, Url, Status"
            );

            return cartDictionary.Values.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while getting the cart details: {ex.Message}");
            throw;
        }
    }
}