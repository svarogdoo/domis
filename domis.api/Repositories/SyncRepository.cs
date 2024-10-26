using domis.api.Models;
using System.Data;
using System.Data.Common;
using Dapper;

namespace domis.api.Repositories;
public interface ISyncRepository
{
    Task<IEnumerable<SalesPoint>> GetSalesPoints();
    Task<bool> SubscribeToNewsletter(string email);
}

public class SyncRepository(IDbConnection connection) : ISyncRepository
{
    public async Task<IEnumerable<SalesPoint>> GetSalesPoints()
    {
        const string sql = @"
                            SELECT id, name, address, phone_numbers AS PhoneNumbers, working_hours AS WorkingHours, 
                            image, google_map_pin AS GoogleMapPin, optional_info AS OptionalInfo 
                            FROM domis.sales_points;";

        var salesPoints = await connection.QueryAsync<SalesPoint>(sql);
        return salesPoints;
    }

    public async Task<bool> SubscribeToNewsletter(string email)
    {
        try
        {
            const string checkQuery = @"
                SELECT COUNT(1) 
                FROM domis.newsletter_subscribers 
                WHERE Email = @Email";
            var alreadySubscribed = await connection.ExecuteScalarAsync<int>(checkQuery, new
            {
                Email = email
            });

            if (alreadySubscribed != 0) return false;
        
            const string insertQuery = @"
                INSERT INTO domis.newsletter_subscribers (Email, SubscribedAt)
                VALUES (@Email, @SubscribedAt)";
            await connection.ExecuteAsync(insertQuery, new
            {
                Email = email, 
                SubscribedAt = DateTime.UtcNow
            });
        
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
