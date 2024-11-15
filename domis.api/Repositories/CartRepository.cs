using System.Data;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Image;
using domis.api.DTOs.Order;
using domis.api.DTOs.Product;
using domis.api.Models;
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
public class CartRepository(IDbConnection connection) : ICartRepository
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

            var sizing = await GetProductSizing(productId);
            var pakSize = await GetPakSize(sizing);
            
            if (addedQuantity % pakSize != 0)
                throw new ArgumentException($"Quantity {addedQuantity} must be in iterations of the pak size {pakSize}.");
            
            var palSize = await GetPalSize(sizing);

            var cartItemExists = await connection.ExecuteScalarAsync<bool>(CartQueries.CheckIfProductExistsInCart, new { CartId = cartId, ProductId = productId });

            return cartItemExists 
                ? await UpdateExistingCartItem(cartId, productId, addedQuantity, role, palSize) 
                : await AddNewCartItem(cartId, productId, addedQuantity, role, palSize);
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
            
            var sizing = await GetProductSizing(ci.ProductId);
            var pakSize = await GetPakSize(sizing);
            if (addedQuantity % pakSize != 0)
                throw new ArgumentException($"Quantity {addedQuantity} must be in iterations of the pak size {pakSize}.");
            
            var totalQ = ci.CurrentQuantity + addedQuantity;
            var palSize = await GetPalSize(sizing);
            
            var rowsAffected = await connection.ExecuteAsync(CartQueries.UpdateCartItemQuantityAndPrice, new
            {
                CartItemId = cartItemId,
                Quantity = totalQ,
                ModifiedAt = DateTime.UtcNow,
                Price = await GetPriceBasedOnRoleAndQuantity(ci.ProductId, role, totalQ, palSize)
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

            var result = await connection.QueryAsync<CartDto, CartItemDto, ProductCartDetailsDto, string, string, CartDto>(
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
    
    private async Task<int?> AddNewCartItem(int? cartId, int productId, decimal quantity, string role, decimal? palSize)
    {
        var price = await GetPriceBasedOnRoleAndQuantity(productId, role, quantity, palSize);
        
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

        var totalQ = addedQuantity + currentQuantity;
        
        if (totalQ < palSize)
        {
            await connection.ExecuteScalarAsync<int>(CartQueries.UpdateCIQuantityByCartAndProduct, new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = totalQ,
                ModifiedAt = DateTime.UtcNow
            });
        }
        else
        {
            await connection.ExecuteScalarAsync<int>(CartQueries.UpdateCIPriceAndQuantityByCartAndProduct, new
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = totalQ,
                Price = GetPriceBasedOnRoleAndQuantity(productId, role, totalQ, palSize),
                ModifiedAt = DateTime.UtcNow
            });
        }

        return cartId;
    }
    
    private async Task<decimal?> GetPriceBasedOnRoleAndQuantity(int productId, string userRole, decimal quantity, decimal? palSize)
    {
        try
        {
            return userRole switch
            {
                "User" or "Admin" => await GetProductPriceRegular(productId),
                "VP1" or "VP2" or "VP3" or "VP4" => await GetProductPriceVp(productId, userRole, quantity, palSize),
                _ => throw new NotSupportedException($"Role '{userRole}' is not supported.")
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving price for product ID {productId} and role {userRole}: {ex.Message}");
            throw;
        }
    }
    
    private async Task<decimal> GetProductPriceRegular(int productId)
    {
        var priceRegular =  await connection.ExecuteScalarAsync<decimal>(ProductQueries.GetProductPrice,
            new { ProductId = productId });

        return priceRegular;
    }

    private async Task<decimal?> GetProductPriceVp(int productId, string role, decimal quantity, decimal? palSize)
    {
        var priceVp = await connection.QueryFirstOrDefaultAsync<VpPriceDetails>(
            ProductQueries.GetSingleProductPricesForVP, 
            new { ProductId = productId, Role = role }
        );

        if (priceVp is null) return null;
        
        if (!palSize.HasValue) return priceVp.PakPrice;
        
        return quantity >= palSize 
            ? priceVp.PalPrice 
            : priceVp.PakPrice;
    }

    private async Task<Size?> GetProductSizing(int productId) 
        => await connection.QuerySingleOrDefaultAsync<Size>(ProductQueries.GetProductSizing, new { ProductId = productId });

    private Task<decimal?> GetPalSize(Size? size)
    {
        decimal? palSize = null;
        if (!string.IsNullOrEmpty(size?.Pal) && decimal.TryParse(size.Pal, out var palValue))
        {
            palSize = palValue;
        }

        return Task.FromResult(palSize);
    }
    
    private Task<decimal?> GetPakSize(Size? size)
    {
        decimal? pakSize = null;
        if (!string.IsNullOrEmpty(size?.Pak) && decimal.TryParse(size.Pak, out var pakValue))
        {
            pakSize = pakValue;
        }

        return Task.FromResult(pakSize);
    }
}