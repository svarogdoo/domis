using domis.api.Models;
using System.Data;
using System.Data.Common;
using Dapper;

namespace domis.api.Repositories;
public interface ISyncRepository
{
    Task<IEnumerable<SalesPoint>> GetSalesPoints();
}

public class SyncRepository(IDbConnection connection) : ISyncRepository
{
    public async Task<IEnumerable<SalesPoint>> GetSalesPoints()
    {
        const string sql = @"SELECT id, name, address, phone_numbers AS PhoneNumbers, working_hours AS WorkingHours, 
                            image, google_map_pin AS GoogleMapPin, optional_info AS OptionalInfo 
                            FROM domis.sales_points;";

        var salesPoints = await connection.QueryAsync<SalesPoint>(sql);
        return salesPoints;
    }
}
