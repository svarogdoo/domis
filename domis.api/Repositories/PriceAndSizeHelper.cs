using System.Data;
using Dapper;
using domis.api.DTOs.Product;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Serilog;

namespace domis.api.Repositories;

public class PriceCalculationHelper(IDbConnection connection)
{
    public async Task<decimal?> GetPriceBasedOnRoleAndQuantity(int productId, string userRole, decimal quantity, Size size)
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
        //Regular users don't get discount on quantity (they don't have pallets)
        //but there is check to see if product is on sale (query)
        var effectivePrice = await connection.ExecuteScalarAsync<decimal>(ProductQueries.GetProductEffectivePrice, new { ProductId = productId });
         
        return effectivePrice;
    }

    private async Task<decimal?> GetProductPriceVp(int productId, string role, decimal packageQuantity, decimal? palSize)
    {
        //vp users don't have products on sale (if you want to include, update query or this method)
        var priceVp = await connection.QueryFirstOrDefaultAsync<VpPriceDetails>(
            ProductQueries.GetSingleProductPricesForVP, 
            new { ProductId = productId, Role = role }
        );

        if (priceVp is null) return null;
        
        if (!palSize.HasValue) return priceVp.PakPrice;
        
        return packageQuantity >= palSize 
            ? priceVp.PalPrice 
            : priceVp.PakPrice;
    }

    public async Task<Size?> GetProductSizing(int productId) 
        => await connection.QuerySingleOrDefaultAsync<Size>(ProductQueries.GetProductSizing, new { ProductId = productId });
    
    public static decimal? PalSizeAsNumber(Size? size)
    {
        if (string.IsNullOrEmpty(size?.Pal)) 
            return null;

        return decimal.TryParse(size.Pal, out var palSize) 
            ? palSize 
            : null;
    }
    
    public static decimal? PakSizeAsNumber(Size? size)
    {
        if (string.IsNullOrEmpty(size?.Pak)) 
            return null;

        return decimal.TryParse(size.Pak, out var pakSize) 
            ? pakSize 
            : null;
    }
}
