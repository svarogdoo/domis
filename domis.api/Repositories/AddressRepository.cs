using System.Data;
using Dapper;
using domis.api.Models;

namespace domis.api.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<AddressEntity>> GetUserAddressesAsync(string userId);
    Task<bool> UpdateAddressAsync(AddressEntity address);
    Task<int> AddAddressAsync(AddressEntity address);
    Task<bool> DeleteAddressAsync(int addressId);
}

public class AddressRepository(IDbConnection connection)  : IAddressRepository
{
    public async Task<IEnumerable<AddressEntity>> GetUserAddressesAsync(string userId)
    {
        const string sql = "SELECT * FROM domis.address WHERE UserId = @UserId";
        return await connection.QueryAsync<AddressEntity>(sql, new { UserId = userId });
    }

    public async Task<bool> UpdateAddressAsync(AddressEntity address)
    {
        const string sql = @"
            UPDATE domis.address
            SET Country = @Country, County = @County, City = @City, 
                AddressLine = @AddressLine, Apartment = @Apartment, PostalCode = @PostalCode, 
                ContactPerson = @ContactPerson, ContactPhone = @ContactPhone, 
            WHERE Id = @Id";
        
        var rows = await connection.ExecuteAsync(sql, address);
        return rows > 0;
    }

    public async Task<int> AddAddressAsync(AddressEntity address)
    {
        const string sql = @"
            INSERT INTO domis.address (UserId, Country, County, City, AddressLine, Apartment, PostalCode, 
                                 ContactPerson, ContactPhone)
            VALUES (@UserId, @Country, @County, @City, @AddressLine, @Apartment, @PostalCode, 
                    @ContactPerson, @ContactPhone)
            RETURNING Id;";
        return await connection.ExecuteScalarAsync<int>(sql, address);
    }

    public Task<bool> DeleteAddressAsync(int addressId)
    {
        throw new NotImplementedException();
    }
}