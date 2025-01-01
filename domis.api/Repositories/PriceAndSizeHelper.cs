using System.Data;
using Dapper;
using domis.api.DTOs.Product;
using domis.api.Repositories.Helpers;
using Serilog;

namespace domis.api.Repositories;

public class PriceAndSizeHelper(IDbConnection connection)
{
    public async Task<decimal?> GetPriceBasedOnRoleAndQuantity(int productId, string userRole, decimal packageQuantity, Size? size)
    {
        try
        {
            return userRole switch
            {
                "User" or "Admin" => await GetProductPriceRegular(productId, size),
                "VP1" or "VP2" or "VP3" or "VP4" => await GetProductPriceVp(productId, userRole, packageQuantity, size),
                _ => throw new NotSupportedException($"Role '{userRole}' is not supported.")
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving price for product ID {productId} and role {userRole}: {ex.Message}");
            throw;
        }
    }
    
    private async Task<decimal?> GetProductPriceRegular(int productId, Size? size)
    {
        //Regular users don't get discount on quantity (they don't have pallets)
        //but there is check to see if product is on sale (query)
        var effectivePrice = await connection.ExecuteScalarAsync<decimal?>(ProductQueries.GetProductEffectivePrice, new { ProductId = productId });
        var pakSize = PakSizeAsNumber(size); 
        
        return effectivePrice * (pakSize ?? 1);
    }

    private async Task<decimal?> GetProductPriceVp(int productId, string role, decimal packageQuantity, Size? size)
    {
        //ovde vratiti cenu paketa za tu odredjenu kolicinu
        
        //vp users don't have products on sale (if you want to include, update query or this method)
        //cenu po jedinici mere za paket i za paletu
        var vpPricePerUnit = await connection.QueryFirstOrDefaultAsync<VpPriceDetails>(
            ProductQueries.GetSingleProductPricesForVP, 
            new { ProductId = productId, Role = role }
        );
        
        //proveriti da li je broj paketa koji je dodat veci ili jednak broju paketa unutar palete
        //ukoliko jeste -> uzmi cenu za pak po jm, ukoliko nije -> uzmi cenu za pal po jm

        var palSize = PalSizeAsNumber(size);
        var pakSize = PakSizeAsNumber(size);
        
        var packagesInPallet = palSize / pakSize;

        return packageQuantity >= packagesInPallet
            ? vpPricePerUnit?.PalPrice * pakSize
            : vpPricePerUnit?.PakPrice * pakSize;
        
        decimal? pricePerPackage;
        if (packageQuantity >= packagesInPallet)
        {
            // koristi cenu za paletu
            pricePerPackage = vpPricePerUnit?.PalPrice * pakSize;
        }
        else
        {
            // koristi cenu za paket
            pricePerPackage = vpPricePerUnit?.PakPrice * pakSize;
        }

        return pricePerPackage;
        
        if (vpPricePerUnit is null) return null;
        
        if (!palSize.HasValue) return vpPricePerUnit.PakPrice;

        // var pricePerPackage = packageQuantity >= palSize
        //     ? vpPricePerUnit.PalPrice * packagesInPallet
        //     : vpPricePerUnit.PakPrice * packagesInPallet;
        //
        // return pricePerPackage;
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
