using Dapper;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface IExchangeRateRepository
{
    Task<bool> UpdateExchangeRate(DateTime date, decimal rate);
}

public class ExchangeRateRepository(IDbConnection connection) : IExchangeRateRepository
{
    public async Task<bool> UpdateExchangeRate(DateTime date, decimal rate)
    {
        var upsertQuery = @"
                INSERT INTO domis.ExchangeRate (Date, Rate) 
                VALUES (@date, @rate)
                ON CONFLICT (Date) 
                DO UPDATE SET Rate = EXCLUDED.Rate;";

        try
        {
            var affectedRows = await connection.ExecuteAsync(upsertQuery, new { date, rate });
            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating ExchangeRate table: {ex.Message}");
            throw;
        }
    }
}
