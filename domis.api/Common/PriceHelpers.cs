using domis.api.Models;
using domis.api.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Common;

public interface IPriceHelpers
{
    Task<decimal> GetDiscount(UserEntity? user);
    Task<bool> IsUserVp(UserEntity? user);
}

public class PriceHelpers(UserManager<UserEntity> userManager, RoleManager<Role> roleManager) : IPriceHelpers
{
    private static readonly Dictionary<string, int> RolePriority = new()
    {
        { "VP4", 0 },
        { "VP3", 1 },
        { "VP2", 2 },
        { "VP1", 3 },
        { "User", 4 }
    };

    public async Task<decimal> GetDiscount(UserEntity? user)
    {
        if (user is null) return 0;

        var roles = await userManager.GetRolesAsync(user);
        if (roles.Count == 0) return 0;

        var userRole = roles
                .Where(role => RolePriority.ContainsKey(role))
                .OrderBy(role => RolePriority[role])
                .FirstOrDefault();

        if (userRole is null) return 0;

        var role = await roleManager.FindByNameAsync(userRole);

        return role?.Discount ?? 0;
    }
    
    public async Task<bool> IsUserVp(UserEntity? user)
    {
        if (user is null) return false;

        var roles = await userManager.GetRolesAsync(user);
        return roles.Any(role => role.StartsWith("VP"));
    }
}

public static class PricingHelper
{
    public static decimal CalculateDiscount(decimal price, decimal discount) 
        => price - (price * discount / 100);
}