using domis.api.DTOs.User;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Repositories;

public interface IUserRepository
{
    Task<UserProfileDto?> GetUserByIdAsync(string id);
}

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<UserProfileDto?> GetUserByIdAsync(string id)
    {
        var identityUser = await userManager.FindByIdAsync(id);

        if (identityUser == null)
            return null;
            
        return new UserProfileDto(identityUser.UserName!, identityUser.Email!);
    }
}