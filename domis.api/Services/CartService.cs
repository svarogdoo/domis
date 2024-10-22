using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ICartService
{
    Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses();
    Task<int> CreateCart(string? userId);
    Task<bool> UpdateCartStatus(int cartId, int statusId);
    Task<int?> CreateCartItem(int? cartId, int productId, decimal quantity, UserEntity? user);
    Task<bool> UpdateCartItemQuantity(int cartItemId, decimal quantity);
    Task<bool> DeleteCartItem(int cartItemId);
    Task<bool> DeleteCart(int cartId);
    Task<CartDto?> GetCart(UserEntity? user, int? cartId);

    Task<bool> SetCartUserId(int cartId, string userId);
}
public class CartService(ICartRepository cartRepository, IPriceHelpers priceHelpers) : ICartService

{
    public async Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses() => 
        await cartRepository.GetAllOrderStatuses();

    public async Task<int> CreateCart(string? userId) =>
        await cartRepository.CreateCartAsync(userId);

    public async Task<bool> UpdateCartStatus(int cartId, int statusId) => 
        await cartRepository.UpdateCartStatusAsync(cartId, statusId);

    public async Task<int?> CreateCartItem(int? cartId, int productId, decimal quantity, UserEntity? user)
    {
        var discount = await priceHelpers.GetDiscount(user);

        return await cartRepository.CreateCartItemAsync(cartId, productId, quantity, user?.Id, discount);
    }

    public async Task<bool> UpdateCartItemQuantity(int cartItemId, decimal quantity) => 
        await cartRepository.UpdateCartItemQuantityAsync(cartItemId, quantity);

    public async Task<bool> DeleteCartItem(int cartItemId) => 
        await cartRepository.DeleteCartItemAsync(cartItemId);

    public async Task<bool> DeleteCart(int cartId) => 
        await cartRepository.DeleteCartAsync(cartId);

    public async Task<bool> SetCartUserId(int cartId, string userId)
        => await cartRepository.SetCartUserId(cartId, userId);

    public async Task<CartDto?> GetCart(UserEntity? user, int? cartId) 
        => await cartRepository.GetCart(user?.Id, cartId);
}