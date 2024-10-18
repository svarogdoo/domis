using domis.api.DTOs.User;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Repositories;

public interface IUserRepository
{
    Task<UserProfileDto?> GetUserByIdAsync(string id);
    Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updated);
    Task<bool> UpdateUserAddressAsync(string id, string address);
}

public class UserRepository(UserManager<UserEntity> userManager) : IUserRepository
{
    public async Task<UserProfileDto?> GetUserByIdAsync(string id)
    {
        var idUser = await userManager.FindByIdAsync(id);

        if (idUser is null || idUser.Email is null)
            return null;

        var user = new UserProfileDto(
            idUser.FirstName ?? string.Empty,
            idUser.LastName ?? string.Empty,
            idUser.AddressLine,
            idUser.City,
            idUser.ZipCode,
            idUser.Country,
            idUser.Email,
            idUser.PhoneNumber
        );

        return user;
    }

    public async Task<bool> UpdateUserAddressAsync(string id, string address)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return false;

        user.Address = address;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updated)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return false;

        user.FirstName = updated.FirstName ?? user.FirstName;
        user.LastName = updated.LastName ?? user.LastName;
        user.AddressLine = updated.AddressLine ?? user.AddressLine;
        user.City = updated.City ?? user.City;
        user.ZipCode = updated.ZipCode ?? user.ZipCode;
        user.Country = updated.Country ?? user.Country;
        user.PhoneNumber = updated.PhoneNumber ?? user.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}