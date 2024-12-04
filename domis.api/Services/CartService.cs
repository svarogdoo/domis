using domis.api.Common;
using domis.api.DTOs.Cart;
using domis.api.DTOs.Order;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ICartService
{
    Task<IEnumerable<OrderStatusDto>?> AllOrderStatuses();
    Task<int> CreateCart(string? userId);
    Task<bool> UpdateCartStatus(int cartId, int statusId);
    Task<int?> CreateCartItem(int? cartId, int productId, decimal quantity, UserEntity? user);
    Task<bool> UpdateCartItemQuantity(int cartItemId, decimal quantity, UserEntity? user);
    Task<bool> DeleteCartItem(int cartItemId);
    Task<bool> DeleteCart(int cartId);
    Task<CartDto?> Cart(UserEntity? user, int? cartId);
    Task<bool> SetCartUserId(int cartId, string userId);
}
public class CartService(ICartRepository cartRepository, IUserRepository userRepo) : ICartService

{
    public async Task<IEnumerable<OrderStatusDto>?> AllOrderStatuses() => 
        await cartRepository.AllOrderStatuses();

    public async Task<int> CreateCart(string? userId) =>
        await cartRepository.CreateCartAsync(userId);

    public async Task<bool> UpdateCartStatus(int cartId, int statusId) => 
        await cartRepository.UpdateCartStatusAsync(cartId, statusId);

    public async Task<int?> CreateCartItem(int? cartId, int productId, decimal quantity, UserEntity? user)
    {
        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id)
            : Roles.User.GetName();
        
        return await cartRepository.CreateCartItemAsync(cartId, productId, quantity, user?.Id, role ?? Roles.User.GetName());
    }

    public async Task<bool> UpdateCartItemQuantity(int cartItemId, decimal quantity, UserEntity? user)
    {        
        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id)
            : Roles.User.GetName();
        
        return await cartRepository.UpdateCartItemQuantityAsync(cartItemId, quantity, role ?? Roles.User.GetName());
    }

    public async Task<bool> DeleteCartItem(int cartItemId) => 
        await cartRepository.DeleteCartItemAsync(cartItemId);

    public async Task<bool> DeleteCart(int cartId) => 
        await cartRepository.DeleteCartAsync(cartId);

    public async Task<bool> SetCartUserId(int cartId, string userId)
        => await cartRepository.SetCartUserId(cartId, userId);

    public async Task<CartDto?> Cart(UserEntity? user, int? cartId)
    {
        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id)
            : Roles.User.GetName();
        
        return await cartRepository.Cart(role ?? Roles.User.GetName(), user?.Id, cartId);
    }
}