using System.Collections;
using AutoMapper;
using Dapper;
using domis.api.Common;
using domis.api.DTOs.Image;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using System.Data;
using System.Data.Common;
using domis.api.DTOs.Category;
using domis.api.DTOs.Common;
using domis.api.Models.Entities;
using ProductBasicInfoDto = domis.api.DTOs.Product.ProductBasicInfoDto;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductDetailsDto?> GetByIdWithDetails(int id, decimal discount);
    Task<ProductDetailsDto?> GetByIdWithDetailsForVp(int id, string role);
    Task<ProductDetailsDto?> Update(ProductUpdateDto product);
    Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
    Task<IEnumerable<SearchResultDto>> SearchProducts(string query, int? pageNumber, int? pageSize);
    Task<bool> PutProductsOnSale(ProductSaleRequest request);
    Task<bool> AssignProductToCategory(AssignProductToCategoryRequest request);
    Task<bool> ProductExists(int productId); 
    Task<IEnumerable<ProductPreviewDto>> GetProductsOnSaleAsync();
    Task<Size?> UpdateProductSizing(int productId, Size updatedSize);
    Task<bool> RemoveProductsFromSale(List<int> productIds);
    Task<IEnumerable<ProductSaleHistoryDto>> GetSaleHistory(int productId);
}

public class ProductRepository(IDbConnection connection, IMapper mapper) : IProductRepository
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
    {
        //TO-DO: check if we need this, and if we do, check if we want to include Featured image as well

        try
        {
            var products = await connection.QueryAsync<ProductPreviewDto>(ProductQueries.GetAll);

            return products;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching products"); throw;
        }
    }

    public async Task<ProductDetailsDto?> GetByIdWithDetails(int productId, decimal discount = 0)
    {
        try
        {
            var product = await connection.QuerySingleOrDefaultAsync<Product>(ProductQueries.GetSingleWithDetails, new { ProductId = productId });
            if (product == null)
                return null;

            var size = await connection.QuerySingleOrDefaultAsync<Size>(ProductQueries.GetProductSizing, new { ProductId = productId });
            var images = (await connection.QueryAsync<ImageGetDto>(ImageQueries.GetProductImages, new { ProductId = productId })).ToList();
            var categoryPaths = await GetProductCategoriesPath(productId);
            
            // Check for an active sale
            var saleEntity = await connection.QuerySingleOrDefaultAsync<SaleEntity>(ProductQueries.GetActiveSale, new 
            { 
                ProductId = productId, 
                CurrentDate = DateTime.UtcNow
            });
            
            var productDetail = mapper.Map<ProductDetailsDto>(product);
            
            productDetail.Size = size;
            productDetail.Images = [.. images];
            productDetail.CategoryPaths = categoryPaths;
            
            if (product.Price.HasValue)
            {
                //if there is a sale on the product
                productDetail.SaleInfo = saleEntity is { SalePrice: not null } 
                    ? SetSaleInfo(saleEntity, size) 
                    : null;
                    
                productDetail.Price = CalculatePakPalPrices(product.Price.Value, size);
            }
            else
            {
                productDetail.Price = null;
            }

            return productDetail;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching product details"); throw;
        }
    }

    public async Task<ProductDetailsDto?> GetByIdWithDetailsForVp(int productId, string role)
    {
        try
        {
            var product = await connection.QuerySingleOrDefaultAsync<Product>(ProductQueries.GetSingleWithDetails, new { ProductId = productId });
            if (product == null)
                return null;

            //product.Price = null; //set to null because for VPs we don't use price from product table (populated from nivelacija.csv)
            var size = await connection.QuerySingleOrDefaultAsync<Size>(ProductQueries.GetProductSizing, new { ProductId = productId });
            var images = (await connection.QueryAsync<ImageGetDto>(ImageQueries.GetProductImages, new { ProductId = productId })).ToList();
            var categoryPaths = await GetProductCategoriesPath(productId);
            var vpPricing = await connection.QueryFirstOrDefaultAsync<VpPriceDetails>(ProductQueries.GetProductPricesForVPMultiple, new { ProductIds = new List<int>{productId}, Role = role });
            
            var productDetail = mapper.Map<ProductDetailsDto>(product);
            
            productDetail.Size = size;
            productDetail.Images = [.. images];
            productDetail.CategoryPaths = categoryPaths;
            productDetail.VpPrice = SetVpPrice(vpPricing, size);
            productDetail.Price = CalculatePakPalPrices(product.Price, size);
            
            //TODO: what to do with sales for VP users???
            
            return productDetail;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching product details"); throw;
        }    
    }

    private static VpPrice SetVpPrice(VpPriceDetails? vpPricing, Size? size)
    {
        if (vpPricing == null || size == null)
        {
            return new VpPrice
            {
                PakUnitPrice = vpPricing?.PakPrice,
                PalUnitPrice = vpPricing?.PalPrice,
                PakPrice = null,
                PalPrice = null
            };
        }

        var pakSize = string.IsNullOrEmpty(size.Pak) ? (decimal?)null : decimal.Parse(size.Pak);
        var palSize = string.IsNullOrEmpty(size.Pal) ? (decimal?)null : decimal.Parse(size.Pal);

        return new VpPrice
        {
            PakUnitPrice = vpPricing.PakPrice,
            PalUnitPrice = vpPricing.PalPrice,
            PakPrice = pakSize.HasValue ? vpPricing?.PakPrice * pakSize.Value : null,
            PalPrice = palSize.HasValue ? vpPricing?.PalPrice * palSize.Value : null
        };
    }


    public async Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId)
    {
        try
        {
            var products = await connection.QueryAsync<ProductBasicInfoDto>(ProductQueries.GetAllByCategory, new { CategoryId = categoryId });
            return products;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching product of a category"); throw;
        }
    }

    public async Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records)
    {
        try
        {
            var result = await connection.ExecuteAsync(ProductQueries.UpdateProductsByNivelacija, records);
            return result > 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating products"); throw;
        }
    }

    public async Task<ProductDetailsDto?> Update(ProductUpdateDto product)
    {
        try
        {
            var exists = await connection.ExecuteScalarAsync<bool>(ProductQueries.CheckIfProductExists, new { ProductId = product.Id });
            
            if (!exists)
                return null;

            var affectedRows = await connection.ExecuteAsync(ProductQueries.UpdateProduct, new
            {
                product.Id,
                product.Title,
                product.IsActive,
                product.Width,
                product.Height,
                product.Weight,
                product.Depth,
                product.Length,
                product.Thickness,
                product.Name,
                product.Description,
                product.Sku,
                product.Price,
                product.Stock,
                product.QuantityType
            });
            
            if (!string.IsNullOrEmpty(product.Pak) || !string.IsNullOrEmpty(product.Pal))
                await UpdateProductSizing(product.Id, new Size {Pak = product.Pak, Pal = product.Pal});

            if (affectedRows != 0) return await GetByIdWithDetails(product.Id);
            
            Log.Warning("Update failed. No rows were affected for ProductId {ProductId}", product.Id);
            return null;
        }
        catch (PostgresException ex) when (ex.SqlState == "23503") // Foreign Key Violation 
        {
            Log.Error(ex, "An error occurred while updating the product with {ProductId}", product.Id); throw;

        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating the product with {ProductId}", product.Id); throw;
        }
    }

    public async Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes()
    {
        try
        {
            var quantityTypes = await connection.QueryAsync<ProductQuantityTypeDto>(ProductQueries.GetAllQuantityTypes);
            return quantityTypes;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching quantity types"); throw;
        }
    }

    public async Task<IEnumerable<SearchResultDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize)
    {
        pageNumber ??= 1;
        pageSize ??= 10;

        var offset = (pageNumber - 1) * pageSize;

        var products = await connection.QueryAsync<SearchResultDto>(
            ProductQueries.SearchByName,
            new
            {
                SearchTerm = "%" + searchTerm + "%",
                PageSize = pageSize,
                Offset = offset
            });
        
        return products;
    }

    public async Task<bool> PutProductsOnSale(ProductSaleRequest request)
    {
        try
        {
            foreach (var productId in  request.ProductIds)
            {
                var exists = await ProductExists(productId);
                if (!exists) continue;

                decimal salePrice;
            
                if (request.SalePrice.HasValue)
                {
                    salePrice = request.SalePrice.Value;
                }
                else
                {
                    var originalPrice = await connection.ExecuteScalarAsync<decimal>(ProductQueries.GetProductPrice, new { ProductId = productId });
                    salePrice = originalPrice - (originalPrice * request.SalePercentage!.Value / 100);
                }
                
                await connection.ExecuteAsync(ProductQueries.DeactivateSale, new { ProductId = productId });
                
                var saleRecord = new
                {
                    ProductId = productId,
                    SalePrice = salePrice,
                    request.StartDate,
                    request.EndDate,
                    IsActive = true
                };
                
                // Insert the sale record into the database for each product
                var affectedRows = await connection.ExecuteAsync(ProductQueries.InsertSale, saleRecord);

                if (affectedRows == 0)
                {
                    Log.Information("Product with id {ProductId} wasn't put on sale.", productId);
                }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while putting the products on sale for request {Request}", request);
            throw;
        }
    }

    public async Task<bool> AssignProductToCategory(AssignProductToCategoryRequest request)
    {
        var affectedRows = await connection.ExecuteAsync(ProductQueries.AssignProductToCategory, new { request.ProductId, request.CategoryId });
        return affectedRows > 0;
    }
    
    public async Task<IEnumerable<ProductSaleHistoryDto>> GetSaleHistory(int productId)
    {
        await connection.ExecuteAsync(ProductQueries.UpdateExpiredSales, new { CurrentTime = DateTime.UtcNow });
        
        return await connection.QueryAsync<ProductSaleHistoryDto>
        (ProductQueries.GetSaleHistory, new
            {
                productId
            }
        );
    }

    #region ExtensionsMethods
    private static Price? CalculatePakPalPrices(decimal? unitPrice, Size? productSize)
    {
        if (unitPrice is null)
            return null;
        
        decimal? pakPrice = null;
        decimal? palPrice = null;

        if (!string.IsNullOrEmpty(productSize?.Pak) && decimal.TryParse(productSize.Pak, out var pakValue))
        {
            pakPrice = unitPrice * pakValue;
        }

        if (!string.IsNullOrEmpty(productSize?.Pal) && decimal.TryParse(productSize.Pal, out var palValue))
        {
            palPrice = unitPrice * palValue;
        }

        return new Price
        {
            Unit = unitPrice,
            Pak = pakPrice,
            Pal = palPrice
        };
    }
    
    private static SaleInfo SetSaleInfo(SaleEntity sale, Size? size)
    {
        var salePricing = CalculatePakPalPrices(sale.SalePrice, size);
        
        return new SaleInfo
        {
            IsActive = true,
            SalePrice = sale.SalePrice,
            SalePakPrice = salePricing?.Pak,
            SalePalPrice = salePricing?.Pal,
            StartDate = sale.StartDate,
            EndDate = sale.EndDate
        };
    }
    
    public async Task<bool> ProductExists(int productId) 
        => await connection.ExecuteScalarAsync<bool>(ProductQueries.CheckIfProductExists, new { ProductId = productId });

    public async Task<IEnumerable<ProductPreviewDto>> GetProductsOnSaleAsync()
    {
        var productsOnSale = await connection.QueryAsync<ProductPreviewDto, SaleInfo, ProductPreviewDto>(
            ProductQueries.GetProductsOnSale,
            (product, saleInfo) =>
            {
                product.SaleInfo = saleInfo is null || !saleInfo.IsActive 
                    ? null
                    : saleInfo;
                return product;
            },
            param: new { CurrentTime = DateTime.UtcNow },
            splitOn: "IsActive"
        );
        
        return productsOnSale;
    }

    public async Task<Size?> UpdateProductSizing(int productId, Size updatedSize)
    {
        var rowsAffected = await connection.ExecuteAsync(ProductQueries.UpdateProductSizing, new
        {
            ProductId = productId,
            updatedSize.Pak,
            updatedSize.Pal,
        });

        return rowsAffected > 0 
            ? new Size { Pak = updatedSize?.Pak, Pal = updatedSize?.Pal }
            : null;
    }

    public async Task<bool> RemoveProductsFromSale(List<int> productIds)
    {
        const string query = @"
            UPDATE domis.sales
            SET is_active = false
            WHERE product_id = ANY(@ProductIds)";
        
        var affectedRows = await connection.ExecuteAsync(query, new { ProductIds = productIds });
        return affectedRows > 0;
    }

    private async Task<List<List<CategoryPath>>> GetProductCategoriesPath(int productId)
    {
        var categoryPathResults = await connection.QueryAsync<ProdCategoryPathRow>(ProductQueries.GetProductCategoriesPaths, new { ProductId = productId });
        var categoryPaths = categoryPathResults
            .Where(row => row is { PathId: not null, Id: not null })
            .GroupBy(row => row.PathId!.Value)
            .Select(group => group.Select(row => new CategoryPath
            {
                Id = row.Id!.Value,
                Name = row.Name
            }).ToList())
            .ToList();
        return categoryPaths;
    }
    
    #endregion ExtensionMethods
}