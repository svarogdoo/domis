using domis.api.DTOs.User;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Repositories;

public interface IUserRepository
{
    Task<IUserProfileDto?> GetUserByIdAsync(string id);
    Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updated);
    Task<bool> UpdateUserAddressAsync(string id, string address);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<string?> GetUserRoleAsync(string userId);
}

public class UserRepository(UserManager<UserEntity> userManager) : IUserRepository
{
    private readonly List<string> _rolePriorities = ["VP4", "VP3", "VP2", "VP1", "Admin", "User"];
    
    public async Task<IUserProfileDto?> GetUserByIdAsync(string id)
    {
        var idUser = await userManager.FindByIdAsync(id);

        if (idUser?.Email is null)
            return null;

        var roles = await userManager.GetRolesAsync(idUser);

        if (roles.Contains(Roles.VP1.RoleName()) || roles.Contains(Roles.VP2.RoleName()) ||
        roles.Contains(Roles.VP3.RoleName()) || roles.Contains(Roles.VP4.RoleName()))
        {
            return new UserWholesaleProfileDto(
                idUser.FirstName!, idUser.LastName!, 
                idUser.AddressLine, idUser.Apartment, idUser.City, idUser.PostalCode, idUser.Country, idUser.County, 
                idUser.Email, idUser.PhoneNumber, idUser.CompanyName
            );
        }

        return new UserProfileDto(
            idUser.FirstName!, idUser.LastName!, 
            idUser.AddressLine, idUser.Apartment, idUser.City, idUser.PostalCode, idUser.Country, idUser.County, 
            idUser.Email, idUser.PhoneNumber
        );
    }

    public async Task<bool> UpdateUserAddressAsync(string id, string address)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return false;

        user.Address = address;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var idUser = await userManager.FindByIdAsync(userId);

        if (idUser?.Email is null)
            return [];

        return await userManager.GetRolesAsync(idUser);    
    }

    public async Task<string?> GetUserRoleAsync(string userId)
    {
        var idUser = await userManager.FindByIdAsync(userId);

        if (idUser?.Email is null)
            return null;

        var roles =  await userManager.GetRolesAsync(idUser);

        return _rolePriorities.FirstOrDefault(r => roles.Contains(r));
    }

    public async Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updated)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return false;

        user.FirstName = updated.FirstName ?? user.FirstName;
        user.LastName = updated.LastName ?? user.LastName;
        user.AddressLine = updated.AddressLine ?? user.AddressLine;
        user.City = updated.City ?? user.City;
        user.PostalCode = updated.PostalCode ?? user.PostalCode;
        user.Country = updated.Country ?? user.Country;
        user.County = updated.County ?? user.County;
        user.Apartment = updated.Apartment ?? user.Apartment;
        user.PhoneNumber = updated.PhoneNumber ?? user.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}