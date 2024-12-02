using domis.api.Models;

namespace domis.api.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<AddressEntity>> GetUserAddressesAsync(string userId);
    Task<AddressEntity?> GetAddressByIdAsync(int addressId);
    Task<bool> UpdateAddressAsync(AddressEntity address);
    Task<int> AddAddressAsync(AddressEntity address);
    Task<bool> DeleteAddressAsync(int addressId);
}

public class AddressRepository
{
    
}