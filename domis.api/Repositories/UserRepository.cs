using domis.api.DTOs.User;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Repositories;

public interface IUserRepository
{
    Task<UserProfileDto?> GetUserByIdAsync(string id);
    Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updated);
    Task<bool> UpdateUserAddressAsync(string id, string address);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<string?> GetUserRoleAsync(string userId);
}

public class UserRepository(
    UserManager<UserEntity> userManager,
    IAddressRepository addressRepo
    ) : IUserRepository
{
    private readonly List<string> _rolePriorities = ["VP4", "VP3", "VP2", "VP1", "Admin", "User"];
    
    public async Task<UserProfileDto?> GetUserByIdAsync(string id)
    {
        var idUser = await userManager.FindByIdAsync(id);

        if (idUser?.Email is null)
            return null;

        var addressesDb = await addressRepo.GetUserAddressesAsync(id);
        var addresses = addressesDb.ToList();

        var company = await GetUserCompany(id);
        
        return new UserProfileDto(
            idUser.FirstName!,
            idUser.LastName!,
            idUser.Email,
            idUser.PhoneNumber,
            null,
            null,
            null
        );
    }

    private async Task<CompanyInfo> GetUserCompany(string id)
    {
        throw new NotImplementedException();
    }

    //TODO: do we need?
    public async Task<bool> UpdateUserAddressAsync(string id, string address)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return false;
        
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
        user.PhoneNumber = updated.PhoneNumber ?? user.PhoneNumber;
        
        //now update company info and two addresses
        //user.CompanyInfo = updated.CompanyInfo ?? user.CompanyInfo;

        var addressDelivery = updated.AddressDelivery;
        var addressInvoice = updated.AddressDelivery;
        var companyInfo = updated.CompanyInfo;
        
        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}