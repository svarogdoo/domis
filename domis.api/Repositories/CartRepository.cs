using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.DTOs.Product;
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
    Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal addedPackageQuantity, string? userId, string role, decimal discount = 0);
    Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal newPackageQuantity, string role);
    Task<bool> DeleteCartItemAsync(int cartItemId);
    Task<bool> DeleteCartAsync(int cartId);
    Task<CartDto?> Cart( string userRole, string? userId, int? cartId);
    Task<bool> SetCartUserId(int cartId, string userId);
}
public class CartRepository(IDbConnection connection, PriceAndSizeHelper helper) : ICartRepository
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
    
    public async Task<int?> CreateCartItemAsync(int? cartId, int productId, decimal addedPackageQuantity, string? userId, string role, decimal discount = 0)
    {
        try
        {
            var existingCartId = await connection.ExecuteScalarAsync<int?>(CartQueries.CheckIfCartExists, new { CartId = cartId, UserId = userId });
            cartId = existingCartId ?? await CreateCartAsync(userId);
            
            var sku = await connection.QuerySingleOrDefaultAsync<int?>(ProductQueries.GetProductSkuById, new { ProductId = productId });
            if (sku is null) throw new NotFoundException($"Product with ID {productId} does not exist.");

            var sizing = await helper.GetProductSizing(productId);
            var pakSize = PriceAndSizeHelper.PakSizeAsNumber(sizing);
            var unitsQuantity = addedPackageQuantity * pakSize;
            
            // if (addedPakQuantity % pakSize != 0 || pakSize == null)
            //     throw new ArgumentException($"Quantity - {addedPakQuantity} must be in increments of the pak size - {pakSize}.");
            
            // var palSize = PriceAndSizeHelper.PalSizeAsNumber(sizing);

            var cartItemExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfProductExistsInCart, new { CartId = cartId, ProductId = productId });

            return cartItemExists
                ? await UpdateExistingCartItem(cartId, productId, addedPackageQuantity, (decimal)unitsQuantity!, role, sizing)
                : await AddNewCartItem(cartId, productId, (int)sku, addedPackageQuantity, (decimal)unitsQuantity!, role, sizing);
            
            // return cartItemExists 
            //     ? await UpdateExistingCartItem(cartId, productId, addedPackageQuantity * (decimal)pakSize!, role, sizing) 
            //     : await AddNewCartItem(cartId, productId, addedPackageQuantity * (decimal)pakSize!, role, sizing);
        }
        catch (Exception ex)
        {
            Log.Error(ex,$"An error occurred while creating a new cart item: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, decimal newPackageQuantity, string role)
    {
        try
        {
            var ci = await connection.QueryFirstOrDefaultAsync<(decimal CurrentQuantity, int ProductId, bool Exists)>(
                CartQueries.GetCartItemProductIdAndQuantity,
                new { CartItemId = cartItemId }
            );
            
            if (!ci.Exists)
                throw new NotFoundException($"Cart item with ID {cartItemId} does not exist.");
            
            var size = await helper.GetProductSizing(ci.ProductId);
            var pakSize = PriceAndSizeHelper.PakSizeAsNumber(size);
            var unitsQuantity = newPackageQuantity * pakSize;
            
            var rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartItemQuantityAndPrice, new
            {
                CartItemId = cartItemId,
                Quantity = newPackageQuantity,
                ModifiedAt = DateTime.UtcNow,
                Price = await helper.GetPriceBasedOnRoleAndQuantity(ci.ProductId, role, newPackageQuantity, size),
                UnitsQuantity = unitsQuantity
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
            if (cart is { Items.Count: > 0 })
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
    
    
    private async Task<int?> AddNewCartItem(int? cartId, int productId, int sku, decimal packageQuantity, decimal unitsQuantity, string role, Size? size)
    {
        var pricePerPackage = await helper.GetPriceBasedOnRoleAndQuantity(productId, role, packageQuantity, size);
        
        var parameters = new
        {
            CartId = cartId,
            ProductId = productId,
            Sku = sku,
            Quantity = packageQuantity,
            Price = pricePerPackage,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            UnitsQuantity = unitsQuantity
        };

        await connection.ExecuteScalarAsync<int>(CartQueries.CreateCartItem, parameters);
        return cartId;
    }

    private async Task<int?> UpdateExistingCartItem(int? cartId, int productId, decimal addedPackageQuantity, decimal unitsQuantity, string role, Size? size)
    {
        var currentPackageQuantity = await connection.ExecuteScalarAsync<decimal>(CartQueries.GetCIQuantityByCartAndProduct, new { CartId = cartId, ProductId = productId });

        //ovde sad moram porediti sa brojem paketa u paleti
        var packagesInPallet = PriceAndSizeHelper.PalSizeAsNumber(size) / PriceAndSizeHelper.PalSizeAsNumber(size);
        
        var totalPackageQuantity = currentPackageQuantity + addedPackageQuantity;
        if (totalPackageQuantity < packagesInPallet)
        {
            await connection.ExecuteScalarAsync<int>(CartQueries.UpdateCIQuantityByCartAndProduct, new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = totalPackageQuantity,
                ModifiedAt = DateTime.UtcNow,
                UnitsQuantity = unitsQuantity
            });
        }
        else
        {
            var updatedPriceForPal = await helper.GetPriceBasedOnRoleAndQuantity(productId, role, totalPackageQuantity, size);
            
            await connection.ExecuteScalarAsync<int>(CartQueries.UpdateCIPriceAndQuantityByCartAndProduct, new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = totalPackageQuantity,
                Price = updatedPriceForPal,
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
    
    //on GET /cart
    private async Task ValidateAndUpdateCartItems(string userRole, List<CartItemDto> cartItems)
    {
        if (cartItems == null || cartItems.Count == 0)
            throw new InvalidOperationException("Cart is empty. Cannot validate items.");

        foreach (var cartItem in cartItems)
        {
            var sizing = await helper.GetProductSizing(cartItem.ProductId);
            
            var expectedPrice = await helper.GetPriceBasedOnRoleAndQuantity(cartItem.ProductId, userRole, cartItem.Quantity, sizing);

            //just for sale
            cartItem.ProductDetails.Price *= PriceAndSizeHelper.PakSizeAsNumber(sizing);
            
            if (expectedPrice == null || cartItem.CartItemPrice == expectedPrice)
                continue;
            
            cartItem.CartItemPrice = (decimal)expectedPrice;
            Log.Information($"Updating price for product ID {cartItem.ProductId}: " +
                            $"Old Price: {cartItem.CartItemPrice}, New Price: {expectedPrice}");
        }
    }

    #endregion
}