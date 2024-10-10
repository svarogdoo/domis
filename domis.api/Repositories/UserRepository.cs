using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(string id);
}

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }
}