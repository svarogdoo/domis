using domis.api.DTOs.Common;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ILocationService
{
    Task<IEnumerable<CountryDto>?> GetAllCountries();
}
public class LocationService(ILocationRepository locationRepository) : ILocationService
{
    public async Task<IEnumerable<CountryDto>?> GetAllCountries() => 
        await locationRepository.GetAllCountries();
}