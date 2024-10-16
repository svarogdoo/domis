using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ICartService
{
    Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses();
    Task<int> CreateCart(string? userId);
    Task<bool> UpdateCartStatus(int cartId, int statusId);
    Task<int?> CreateCartItem(int? cartId, int productId, decimal quantity, string? userId);
    Task<bool> UpdateCartItemQuantity(int cartItemId, decimal quantity);
    Task<bool> DeleteCartItem(int cartItemId);
    Task<bool> DeleteCart(int cartId);
    Task<CartDto?> GetCartWithItemsAndProductDetails(int cartId);
    Task<CartDto?> GetCartByUserId(string userId);
}
public class CartService(ICartRepository cartRepository) : ICartService

{
    public async Task<IEnumerable<OrderStatusDto>?> GetAllOrderStatuses() => 
        await cartRepository.GetAllOrderStatuses();

    public async Task<int> CreateCart(string? userId) =>
        await cartRepository.CreateCartAsync(userId);

    public async Task<bool> UpdateCartStatus(int cartId, int statusId) => 
        await cartRepository.UpdateCartStatusAsync(cartId, statusId);
    
    public async Task<int?> CreateCartItem(int? cartId, int productId, decimal quantity, string? userId) => 
        await cartRepository.CreateCartItemAsync(cartId, productId, quantity, userId);

    public async Task<bool> UpdateCartItemQuantity(int cartItemId, decimal quantity) => 
        await cartRepository.UpdateCartItemQuantityAsync(cartItemId, quantity);

    public async Task<bool> DeleteCartItem(int cartItemId) => 
        await cartRepository.DeleteCartItemAsync(cartItemId);

    public async Task<bool> DeleteCart(int cartId) => 
        await cartRepository.DeleteCartAsync(cartId);

    public async Task<CartDto?> GetCartWithItemsAndProductDetails(int cartId) => 
        await cartRepository.GetCartWithItemsAndProductDetailsAsync(cartId);

    public async Task<CartDto?> GetCartByUserId(string userId) 
        => await cartRepository.GetCartWithItemsAndProductDetailsAsyncByUserId(userId);
}