using domis.api.DTOs.User;
using domis.api.Models;
using domis.api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Services;

public interface IUserService
{
    Task<UserProfileDto?> GetUserByIdAsync(string id);
}

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserProfileDto?> GetUserByIdAsync(string id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }
}