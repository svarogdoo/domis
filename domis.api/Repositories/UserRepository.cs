using domis.api.DTOs.User;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Repositories;

public interface IUserRepository
{
    Task<UserProfileDto?> GetUserByIdAsync(string id);
    Task<bool> UpdateUserAsync(string id, ProfileUpdateRequest updated);
    Task<bool> UpdateUserAddressAsync(string id, string address);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<string?> GetUserRoleAsync(string userId);
}

public class UserRepository(
    UserManager<UserEntity> userManager,
    IUserExtensionRepository extensionRepo
    ) : IUserRepository
{
    private readonly List<string> _rolePriorities = ["VP4", "VP3", "VP2", "VP1", "Admin", "User"];
    
    public async Task<UserProfileDto?> GetUserByIdAsync(string id)
    {
        var idUser = await userManager.FindByIdAsync(id);

        if (idUser?.Email is null)
            return null;

        var addressesDb = await extensionRepo.GetAddressesAsync(id);
        var addresses = addressesDb.ToList();
        var deliveryAddress = addresses.FirstOrDefault(a => a.AddressType == AddressType.Delivery);
        var invoiceAddress = addresses.FirstOrDefault(a => a.AddressType == AddressType.Invoice);

        var company = await extensionRepo.GetCompanyInfoAsync(id);
        
        return new UserProfileDto(
            idUser.FirstName!,
            idUser.LastName!,
            idUser.Email,
            idUser.PhoneNumber,
            company,
            deliveryAddress,
            invoiceAddress
        );
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

    public async Task<bool> UpdateUserAsync(string id, ProfileUpdateRequest updated)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return false;

        user.FirstName = updated.FirstName ?? user.FirstName;
        user.LastName = updated.LastName ?? user.LastName;
        user.PhoneNumber = updated.PhoneNumber ?? user.PhoneNumber;

        await SetCompany(id, updated);
        await SetAddresses(id, updated);

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    private async Task SetCompany(string id, ProfileUpdateRequest updated)
    {
        if (updated.CompanyInfo != null)
        {
            await extensionRepo.UpdateOrCreateCompanyAsync(id, updated.CompanyInfo);
        }
    }

    private async Task SetAddresses(string userId, ProfileUpdateRequest updated)
    {
        if (updated.UseSameAddress)
        {
            if (updated.AddressInvoice != null)
            {
                await extensionRepo.UpdateOrCreateAddressAsync(userId, updated.AddressInvoice, "Delivery");
                await extensionRepo.UpdateOrCreateAddressAsync(userId, updated.AddressInvoice, "Invoice");
            }
            else if (updated.AddressDelivery != null)
            {
                await extensionRepo.UpdateOrCreateAddressAsync(userId, updated.AddressDelivery, "Delivery");
                await extensionRepo.UpdateOrCreateAddressAsync(userId, updated.AddressDelivery, "Invoice");
            }
        }
        else
        {
            if (updated.AddressDelivery != null)
            {
                await extensionRepo.UpdateOrCreateAddressAsync(userId, updated.AddressDelivery, "Delivery");
            }

            if (updated.AddressInvoice != null)
            {
                await extensionRepo.UpdateOrCreateAddressAsync(userId, updated.AddressInvoice, "Invoice");
            }
        }
    }
}