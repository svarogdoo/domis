using System.Data;
using Dapper;
using domis.api.DTOs.User;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories.Queries;

namespace domis.api.Repositories;

public interface IUserExtensionRepository
{
    Task<IEnumerable<AddressProfileDto>> GetAddressesAsync(string userId);
    Task<bool> UpdateAddressAsync(AddressEntity address);
    Task<int> AddAddressAsync(AddressEntity address);
    Task<bool> DeleteAddressAsync(string userId, string addressType);
    Task<CompanyProfileDto?> GetCompanyInfoAsync(string userId);
    Task<bool> UpdateOrCreateCompanyAsync(string userId, ProfileCompanyUpdateRequest company);
    Task<bool> UpdateOrCreateAddressAsync(string userId, ProfileAddressUpdateRequest address, string type);
}

public class UserExtensionRepository(IDbConnection connection)  : IUserExtensionRepository
{
    public async Task<IEnumerable<AddressProfileDto>> GetAddressesAsync(string userId)
    {
        return await connection.QueryAsync<AddressProfileDto>(UserExtensionsQueries.GetAddresses, new { UserId = userId });
    }
    
    public async Task<CompanyProfileDto?> GetCompanyInfoAsync(string userId)
    {
        return await connection.QuerySingleOrDefaultAsync<CompanyProfileDto>(UserExtensionsQueries.GetCompany, new { UserId = userId });
    }

    public async Task<bool> UpdateOrCreateCompanyAsync(string userId, ProfileCompanyUpdateRequest company)
    {
        var companyId = await connection.ExecuteScalarAsync<int>(UserExtensionsQueries.UpsertCompany, new 
        { 
            userid = userId,
            company.Name,
            company.Number,
            company.FirstName,
            company.LastName
        });

        return companyId > 0;
    }


    public async Task<bool> UpdateOrCreateAddressAsync(string userId, ProfileAddressUpdateRequest address, string addressType)
    {
        var addressId = await connection.ExecuteScalarAsync<int>(UserExtensionsQueries.UpsertAddress, new 
        { 
            UserId = userId,
            address.Country,
            address.County,
            address.City,
            address.AddressLine,
            address.Apartment,
            address.PostalCode,
            address.ContactPerson,
            address.ContactPhone,
            AddressType = addressType
        });

        return addressId > 0;
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

    public async Task<bool> DeleteAddressAsync(string userId, string addressType)
    {
        const string sql = "DELETE FROM domis.address WHERE userid = @UserId AND AddressType = @AddressType";
        var rows = await connection.ExecuteAsync(sql, new { UserId = userId, AddressType = addressType });
        return rows > 0;
    }
}