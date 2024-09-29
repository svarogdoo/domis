using System.Data;
using Dapper;
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
    Task<int> CreateCartAsync(int? userId);
    Task<bool> UpdateCartStatusAsync(int cartId, int statusId);
    Task<int?> CreateCartItemAsync(int cartId, int productId, decimal quantity);
    Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal quantity);
    Task<bool> DeleteCartItemAsync(int cartItemId);
    Task<bool> DeleteCartAsync(int cartId);
    Task<CartDto?> GetCartWithItemsAndProductDetailsAsync(int cartId);

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
    
    public async Task<int> CreateCartAsync(int? userId)
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
    
    public async Task<int?> CreateCartItemAsync(int cartId, int productId, decimal quantity)
    {
        try
        {
            var cartExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfCartExists, new { CartId = cartId });
            if (!cartExists) return null;
            //{
            //    throw new NotFoundException($"Cart with ID {cartId} does not exist.");
            //}

            var productExists = await connection.ExecuteScalarAsync<bool>(ProductQueries.CheckIfProductExists, new { ProductId = productId });
            if (!productExists) return null;
            //{
            //    throw new NotFoundException($"Product with ID {productId} does not exist.");
            //}

            var parameters = new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };


            var newCartItemId = await connection.ExecuteScalarAsync<int>(CartQueries.CreateCartItem, parameters);

            return newCartItemId;
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
    
    public async Task<CartDto?> GetCartWithItemsAndProductDetailsAsync(int cartId)
    {
        try
        {
            var cartDictionary = new Dictionary<int, CartDto>();

            var result = await connection.QueryAsync<CartDto, CartItemDto, ProductCartDetailsDto, string, string, CartDto>(
                CartQueries.GetCart,
                (cart, item, product, image, status) =>
                {
                    if (!cartDictionary.TryGetValue(cart.CartId, out var currentCart))
                    {
                        currentCart = cart;
                        currentCart.Items = [];
                        cartDictionary.Add(currentCart.CartId, currentCart);
                    }

                    if (item is not null 
                        && currentCart.Items.Find(i => i.CartItemId == item.CartItemId) == null)
                    {
                        item.ProductDetails = product;
                        item.ProductDetails.Image = image;

                        currentCart.Items.Add(item);
                    }

                    currentCart.Status = status; // Assign the status to the cart

                    return currentCart;
                },
                new { CartId = cartId },
                splitOn: "CartItemId, Name, Url, Status"
            );

            return cartDictionary.Values.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while getting the cart details: {ex.Message}");
            throw; 
        }
    }
}