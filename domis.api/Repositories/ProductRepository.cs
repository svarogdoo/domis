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

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductDetailsDto?> GetByIdWithDetails(int id, decimal discount);
    Task<ProductDetailsDto?> Update(ProductUpdateDto product);
    Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
    Task<IEnumerable<ProductBasicInfoDto>> SearchProducts(string query, int? pageNumber, int? pageSize);
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
            var categoryPaths = (await connection.QueryAsync<string>(CategoryQueries.GetProductCategories, new { ProductId = productId })).ToList();

            var productDetail = mapper.Map<ProductDetailsDto>(product);
            
            productDetail.Size = size;
            productDetail.Images = [.. images];
            productDetail.CategoryPaths = [.. categoryPaths];
            if (product.Price.HasValue)
            {
                var discountedPrice = PricingHelper.CalculateDiscount(product.Price.Value, discount);
                productDetail.Price = CalculatePriceByPackage(discountedPrice, size);
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

            if (affectedRows == 0)
            {
                Log.Warning("Update failed. No rows were affected for ProductId {ProductId}", product.Id);
                return null;
            }

            //var updatedProduct = await connection.QuerySingleOrDefaultAsync<ProductCompleteDetailsDto>(ProductQueries.GetSingleWithDetails, new { ProductId = product.Id });

            var updatedProduct = await GetByIdWithDetails(product.Id);

            //var updatedProduct = await connection.QuerySingleOrDefaultAsync<ProductEditDto>(ProductQueries.GetById, new { ProductId = product.Id });
            return updatedProduct;
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

    public async Task<IEnumerable<ProductBasicInfoDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize)
    {
        pageNumber ??= 1;
        pageSize ??= 10;

        var offset = (pageNumber - 1) * pageSize;

        const string query = @"
            SELECT id AS Id, product_name AS Name, sku AS Sku
            FROM domis.product
            WHERE product_name ILIKE @SearchTerm OR CAST(sku AS TEXT) ILIKE @SearchTerm
            LIMIT @PageSize OFFSET @Offset
        ";

        var products = await connection.QueryAsync<ProductBasicInfoDto>(
                query,
                new
                {
                    SearchTerm = "%" + searchTerm + "%",
                    PageSize = pageSize,
                    Offset = offset
                });
        
        return products;
    }

    #region ExtensionsMethods
    private static Price CalculatePriceByPackage(decimal unitPrice, Size? productSize)
    {
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
    #endregion ExtensionMethods
}