using domis.api.DTOs.User;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IUserService
{
    Task<UserProfileDto?> UserProfile(string id);
    Task<bool> UpdateUserAsync(string id, ProfileUpdateRequest updateDto);
    Task<bool> UpdateUserAddressAsync(string id, string address);
    Task<IEnumerable<string>> Roles(string userId);
    Task<string?> Role(string userId);
}

public class UserService(IUserRepository userRepository, IOrderRepository orderRepository) : IUserService
{
    public async Task<UserProfileDto?> UserProfile(string id) 
        => await userRepository.GetUserByIdAsync(id);

    public async Task<bool> UpdateUserAddressAsync(string id, string address)
        => await userRepository.UpdateUserAddressAsync(id, address);

    public async Task<IEnumerable<string>> Roles(string userId)
        => await userRepository.GetUserRolesAsync(userId);

    public async Task<string?> Role(string userId)
        => await userRepository.GetUserRoleAsync(userId);

    public async Task<bool> UpdateUserAsync(string id, ProfileUpdateRequest updateDto)
        => await userRepository.UpdateUserAsync(id, updateDto);
}