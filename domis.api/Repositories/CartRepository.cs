using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Serilog;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace domis.api.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<OrderStatusDto>?> AllOrderStatuses();
    Task<int> CreateCartAsync(string? userId);
    Task<bool> UpdateCartStatusAsync(int cartId, int statusId);
    Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal addedPakQuantity, string? userId, string role, decimal discount = 0);
    Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal newPakQuantity, string role);
    Task<bool> DeleteCartItemAsync(int cartItemId);
    Task<bool> DeleteCartAsync(int cartId);
    Task<CartDto?> Cart( string userRole, string? userId, int? cartId);
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
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DateTimeHelper.BelgradeTimeZone)
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
    
    public async Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal addedPakQuantity, string? userId, string role, decimal discount = 0)
    {
        try
        {
            var cartExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfCartExists, new { CartId = cartId });
            if (!cartExists) cartId = await CreateCartAsync(userId);

            var productExists = await connection.ExecuteScalarAsync<bool>(ProductQueries.CheckIfProductExists, new { ProductId = productId });
            if (!productExists) throw new NotFoundException($"Product with ID {productId} does not exist.");

            var sizing = await helper.GetProductSizing(productId);
            var pakSize = PriceCalculationHelper.PakSizeAsNumber(sizing);
            
            // if (addedPakQuantity % pakSize != 0 || pakSize == null)
            //     throw new ArgumentException($"Quantity - {addedPakQuantity} must be in increments of the pak size - {pakSize}.");
            
            var palSize = PriceCalculationHelper.PalSizeAsNumber(sizing);

            var cartItemExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfProductExistsInCart, new { CartId = cartId, ProductId = productId });

            return cartItemExists 
                ? await UpdateExistingCartItem(cartId, productId, addedPakQuantity * (decimal)pakSize!, role, palSize) 
                : await AddNewCartItem(cartId, productId, addedPakQuantity * (decimal)pakSize!, role, palSize);
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while creating a new cart item: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal newPakQuantity, string role)
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
            var pakSize = decimal.Parse(sizing.Pak);
            
            // if (addedQuantity % pakSize != 0)
            //     throw new ArgumentException($"Quantity {addedQuantity} must be in increments of the pak size {pakSize}.");
            
            // var totalQ = ci.CurrentQuantity + addedQuantity;
            // var totalQ = newQuantity;
            var palSize = PriceCalculationHelper.PalSizeAsNumber(sizing);
            
            var rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartItemQuantityAndPrice, new
            {
                CartItemId = cartItemId,
                Quantity = newPakQuantity,
                ModifiedAt = DateTime.UtcNow,
                Price = await helper.GetPriceBasedOnRoleAndQuantity(ci.ProductId, role, newPakQuantity * pakSize, palSize)
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

    public async Task<CartDto?> Cart(string userRole, string? userId = null, int? cartId = null)
    {
        try
        {
            var cartDictionary = new Dictionary<int, CartDto>();

            var (getCartQuery, getCartParams) = SetCartQueryAndParams(userId, cartId);

            await connection.QueryAsync<CartDto, CartItemDto, ProductCartDetailsDto, string, string, CartDto>(
                getCartQuery,
                (cart, item, product, image, status) =>
                {
                    if (!cartDictionary.TryGetValue(cart.CartId, out var currentCart))
                    {
                        currentCart = cart;
                        currentCart.Items = []; // Initialize the Items list properly
                        cartDictionary.Add(currentCart.CartId, currentCart);
                    }

                    if (item is not null && currentCart.Items.Find(i => i.CartItemId == item.CartItemId) == null)
                    {
                        item.ProductDetails = product;
                        item.ProductDetails.Image = image;
                        item.CartItemPrice = item.CartItemPrice;
                        //item.ProductDetails.Price = PricingHelper.CalculateDiscount(item.ProductDetails.Price, discount);
                        
                        currentCart.Items.Add(item);
                    }

                    currentCart.Status = status;
                    return currentCart;
                },
                getCartParams,
                splitOn: "CartItemId, Name, Url, Status"
            );

            var cart = cartDictionary.Values.FirstOrDefault();
            if (cart != null)
                await ValidateAndUpdateCartItems(userRole, cart.Items);

            return cart;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while getting the cart details: {ex.Message}");
            throw;
        }
    }
    #region ExtensionMethods
    
    
    private async Task<int?> AddNewCartItem(int? cartId, int productId, decimal quantity, string role, decimal? palSize)
    {
        var price = await helper.GetPriceBasedOnRoleAndQuantity(productId, role, quantity, palSize);
        
        var parameters = new
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            Price = price,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        await connection.ExecuteScalarAsync<int>(CartQueries.CreateCartItem, parameters);
        return cartId;
    }

    private async Task<int?> UpdateExistingCartItem(int? cartId, int productId, decimal addedQuantity, string role, decimal? palSize)
    {
        var currentQuantity = await connection.ExecuteScalarAsync<decimal>(CartQueries.GetCIQuantityByCartAndProduct, new { CartId = cartId, ProductId = productId });

        var totalQuantity = currentQuantity + addedQuantity;
        if (totalQuantity < palSize)
        {
            await connection.ExecuteScalarAsync<int>(CartQueries.UpdateCIQuantityByCartAndProduct, new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = totalQuantity,
                ModifiedAt = DateTime.UtcNow
            });
        }
        else
        {
            var updatedPrice = await helper.GetPriceBasedOnRoleAndQuantity(productId, role, totalQuantity, palSize);
            
            await connection.ExecuteScalarAsync<int>(CartQueries.UpdateCIPriceAndQuantityByCartAndProduct, new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = totalQuantity,
                Price = updatedPrice,
                ModifiedAt = DateTime.UtcNow
            });
        }

        return cartId;
    }
    
    private static (string query, object parameters) SetCartQueryAndParams(string? userId, int? cartId)
    {
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

        return (query, parameters);
    }
    
    private async Task ValidateAndUpdateCartItems(string userRole, List<CartItemDto> cartItems)
    {
        if (cartItems == null || cartItems.Count == 0)
            throw new InvalidOperationException("Cart is empty. Cannot validate items.");

        foreach (var cartItem in cartItems)
        {
            var sizing = await helper.GetProductSizing(cartItem.ProductId);
            var palSize = PriceCalculationHelper.PalSizeAsNumber(sizing);

            var expectedPrice = 
                await helper.GetPriceBasedOnRoleAndQuantity(cartItem.ProductId, userRole, cartItem.Quantity, palSize);

            if (expectedPrice == null || cartItem.CartItemPrice == expectedPrice) continue;
            
            // If the price has changed, update it
            Log.Information($"Updating price for product ID {cartItem.ProductId}: " +
                            $"Old Price: {cartItem.CartItemPrice}, New Price: {expectedPrice}");

            cartItem.CartItemPrice = (decimal)expectedPrice;
        }
    }

    #endregion
}