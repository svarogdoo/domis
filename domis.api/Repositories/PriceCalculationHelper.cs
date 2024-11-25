using System.Data;
using Dapper;
using domis.api.DTOs.Product;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Serilog;

namespace domis.api.Repositories;

public class PriceCalculationHelper(IDbConnection connection)
{
    public async Task<decimal?> GetPriceBasedOnRoleAndQuantity(int productId, string userRole, decimal quantity, decimal? palSize)
    {
        try
        {
            return userRole switch
            {
                "User" or "Admin" => await GetProductPriceRegular(productId),
                "VP1" or "VP2" or "VP3" or "VP4" => await GetProductPriceVp(productId, userRole, quantity, palSize),
                _ => throw new NotSupportedException($"Role '{userRole}' is not supported.")
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving price for product ID {productId} and role {userRole}: {ex.Message}");
            throw;
        }
    }
    
    private async Task<decimal> GetProductPriceRegular(int productId)
    {
        
        var effectivePrice = await connection.ExecuteScalarAsync<decimal>(ProductQueries.GetProductEffectivePrice,
        new { ProductId = productId });
         
        return effectivePrice;
    }

    private async Task<decimal?> GetProductPriceVp(int productId, string role, decimal quantity, decimal? palSize)
    {
        var priceVp = await connection.QueryFirstOrDefaultAsync<VpPriceDetails>(
            ProductQueries.GetSingleProductPricesForVP, 
            new { ProductId = productId, Role = role }
        );

        if (priceVp is null) return null;
        
        if (!palSize.HasValue) return priceVp.PakPrice;
        
        return quantity >= palSize 
            ? priceVp.PalPrice 
            : priceVp.PakPrice;
    }

    public async Task<Size?> GetProductSizing(int productId) 
        => await connection.QuerySingleOrDefaultAsync<Size>(ProductQueries.GetProductSizing, new { ProductId = productId });
    
    public decimal? PalSizeAsNumber(Size? size)
    {
        if (string.IsNullOrEmpty(size?.Pal)) 
            return null;

        return decimal.TryParse(size.Pal, out var palSize) 
            ? palSize 
            : null;
    }
    
    public decimal? PakSizeAsNumber(Size? size)
    {
        if (string.IsNullOrEmpty(size?.Pak)) 
            return null;

        return decimal.TryParse(size.Pak, out var pakSize) 
            ? pakSize 
            : null;
    }
}
