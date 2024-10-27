using System.Data;
using Dapper;
using domis.api.DTOs.Common;
using domis.api.Repositories.Queries;
using Serilog;

namespace domis.api.Repositories;

public interface ILocationRepository
{
    Task<IEnumerable<CountryDto>?> GetAllCountries();
}
public class LocationRepository(IDbConnection connection) : ILocationRepository
{
    public async Task<IEnumerable<CountryDto>?> GetAllCountries()
    {
        try
        {
            var countries =
                (await connection.QueryAsync<CountryDto>(CommonQueries.GetCountries))
                .ToList();

            return countries.Count != 0 ? countries : null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching countries: {ex.Message}");
            throw;
        }
    }
}