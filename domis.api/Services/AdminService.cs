using domis.api.DTOs.User;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace domis.api.Services;

public interface IAdminService
{
    Task<UserWithRolesDto?> PromoteToAdmin(string userId);
    Task<List<UserWithRolesDto>> GetUsers();
    Task<UserWithRoles?> GetUserById(string userId);
    Task<List<Role>> GetRoles();
    Task<Role?> UpdateRoleDiscount(string roleName, decimal discount);
    Task<bool> AddRoleToUser(string userId, Roles role);
    Task<bool> RemoveRoleFromUser(string userId, Roles role);
    
    Task<IEnumerable<OrderDetailsDto>> Orders();
}

public class AdminService(
    UserManager<UserEntity> userManager, 
    RoleManager<Role> roleManager, 
    IOrderRepository orderRepository)
    : IAdminService
{
    public async Task<UserWithRolesDto?> PromoteToAdmin(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return null;

        if (!await roleManager.RoleExistsAsync(Roles.Admin.GetName()))
            return null;

        var userRoles = await userManager.GetRolesAsync(user);
        if (userRoles.Contains(Roles.User.GetName()))
            await userManager.RemoveFromRoleAsync(user, Roles.User.GetName());

        var result = await userManager.AddToRoleAsync(user, Roles.Admin.GetName());
        if (!result.Succeeded)
            return null;

        var roles = await userManager.GetRolesAsync(user);
        return new UserWithRolesDto
        {
            UserId = user.Id, UserName = user.UserName, Roles = roles.ToList() 
            
        };
    }

    public async Task<List<UserWithRolesDto>> GetUsers()
    {
        var users = await userManager.Users.ToListAsync();
        var usersWithRoles = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            usersWithRoles.Add(new UserWithRolesDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Roles = roles.ToList(),
                Role = roles.FirstOrDefault() ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        return usersWithRoles;
    }

    public async Task<UserWithRoles?> GetUserById(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await userManager.GetRolesAsync(user);
        return new UserWithRoles
        {
            User = user,
            Roles = roles.ToList()
        };
    }

    public async Task<List<Role>> GetRoles()
    {
        return await roleManager.Roles.ToListAsync();
    }

    public async Task<Role?> UpdateRoleDiscount(string roleName, decimal discount)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null) return null;

        role.Discount = discount;
        var result = await roleManager.UpdateAsync(role);
        return result.Succeeded 
            ? role 
            : null;
    }

    public async Task<bool> AddRoleToUser(string userId, Roles role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var roleName = role.GetName();
        if (!await roleManager.RoleExistsAsync(roleName))
            return false;

        var userRoles = await userManager.GetRolesAsync(user);

        if (userRoles.Contains(roleName))
            return true;

        // Remove all current roles from the user
        foreach (var userRole in userRoles)
        {
            await userManager.RemoveFromRoleAsync(user, userRole);
        }

        var result = await userManager.AddToRoleAsync(user, roleName);

        return result.Succeeded;
    }


    public async Task<bool> RemoveRoleFromUser(string userId, Roles role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) 
            return false;

        var roleName = role.GetName();
        if (!await roleManager.RoleExistsAsync(roleName))
            return false;

        var userRoles = await userManager.GetRolesAsync(user);
        if (!userRoles.Contains(roleName))
            return false;

        var result = await userManager.RemoveFromRoleAsync(user, roleName);
        
        if (result.Succeeded)         //default to 'User' role
            await userManager.AddToRoleAsync(user, Roles.User.GetName());
        
        return result.Succeeded;
    }

    public async Task<IEnumerable<OrderDetailsDto>> Orders() 
        => await orderRepository.GetOrders();
}
