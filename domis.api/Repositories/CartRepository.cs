using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Serilog;

namespace domis.api.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<OrderStatusDto>?> AllOrderStatuses();
    Task<int> CreateCartAsync(string? userId);
    Task<bool> UpdateCartStatusAsync(int cartId, int statusId);
    Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal addedQuantity, string? userId, string role, decimal discount = 0);
    Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal addedQuantity, string role);
    Task<bool> DeleteCartItemAsync(int cartItemId);
    Task<bool> DeleteCartAsync(int cartId);
    Task<CartDto?> Cart(string? userId, int? cartId);
    Task<bool> SetCartUserId(int cartId, string userId);
}
public class CartRepository(IDbConnection connection, PriceCalculationHelper helper) : ICartRepository
{
    public async Task<IEnumerable<OrderStatusDto>?> AllOrderStatuses()
    {
        try
        {
            var orderStatuses = 
                (await connection.QueryAsync<OrderStatusDto>(CartQueries.GetAllCartStatuses))
                .ToList();

            return orderStatuses.Count != 0 ?  orderStatuses: null;
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
                StatusId = 1, //Active
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

            var rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartStatus, parameters);
            
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating the cart status: {ex.Message}");
            throw;
        }
    }
    
    public async Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal addedQuantity, string? userId, string role, decimal discount = 0)
    {
        try
        {
            var cartExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfCartExists, new { CartId = cartId });
            if (!cartExists) cartId = await CreateCartAsync(userId);

            var productExists = await connection.ExecuteScalarAsync<bool>(ProductQueries.CheckIfProductExists, new { ProductId = productId });
            if (!productExists) throw new NotFoundException($"Product with ID {productId} does not exist.");

            var sizing = await helper.GetProductSizing(productId);
            var pakSize = helper.PakSizeAsNumber(sizing);
            
            if (addedQuantity % pakSize != 0)
                throw new ArgumentException($"Quantity - {addedQuantity} must be in increments of the pak size - {pakSize}.");
            
            var palSize = helper.PalSizeAsNumber(sizing);

            var cartItemExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfProductExistsInCart, new { CartId = cartId, ProductId = productId });

            return cartItemExists 
                ? await helper.UpdateExistingCartItem(cartId, productId, addedQuantity, role, palSize) 
                : await helper.AddNewCartItem(cartId, productId, addedQuantity, role, palSize);
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while creating a new cart item: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal addedQuantity, string role)
    {
        try
        {
            var ci = await connection.QueryFirstOrDefaultAsync<(decimal CurrentQuantity, int ProductId, bool Exists)>(
                CartQueries.GetCartItemProductIdAndQuantity,
                new { CartItemId = cartItemId }
            );
            
            if (!ci.Exists)
                throw new NotFoundException($"Cart item with ID {cartItemId} does not exist.");
            
            var sizing = await helper.GetProductSizing(ci.ProductId);
            var pakSize = helper.PakSizeAsNumber(sizing);
            if (addedQuantity % pakSize != 0)
                throw new ArgumentException($"Quantity {addedQuantity} must be in increments of the pak size {pakSize}.");
            
            var totalQ = ci.CurrentQuantity + addedQuantity;
            var palSize = helper.PalSizeAsNumber(sizing);
            
            var rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartItemQuantityAndPrice, new
            {
                CartItemId = cartItemId,
                Quantity = totalQ,
                ModifiedAt = DateTime.UtcNow,
                Price = await helper.GetPriceBasedOnRoleAndQuantity(ci.ProductId, role, totalQ, palSize)
            });

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

            var rowsAffected = await connection.ExecuteAsync(CartQueries.DeleteCartItem, parameters);

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
            var rowsAffected = await connection.ExecuteAsync(CartQueries.DeleteCartQuery, parameters, transaction);

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
    
    //TODO: do we need this? 
    public Task<bool> SetCartUserId(int cartId, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartDto?> Cart(string? userId = null, int? cartId = null)
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

            await connection.QueryAsync<CartDto, CartItemDto, ProductCartDetailsDto, string, string, CartDto>(
                query,
                (cart, item, product, image, status) =>
                {
                    if (!cartDictionary.TryGetValue(cart.CartId, out var currentCart))
                    {
                        currentCart = cart;
                        currentCart.Items = []; // Initialize the Items list properly
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