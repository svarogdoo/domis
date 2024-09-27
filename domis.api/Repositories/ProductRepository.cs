using AutoMapper;
using Dapper;
using domis.api.DTOs.Image;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductCompleteDetailsDto?> GetByIdWithDetails(int id);
    Task<ProductCompleteDetailsDto?> Update(ProductEditDto product);
    Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
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

    public async Task<ProductCompleteDetailsDto?> GetByIdWithDetails(int productId)
    {
        try
        {
            var product = await connection.QuerySingleOrDefaultAsync<Product>(ProductQueries.GetSingleWithDetails, new { ProductId = productId });
            if (product == null)
                return null;

            var images = (await connection.QueryAsync<ImageGetDto>(ImageQueries.GetProductImages, new { ProductId = productId })).ToList();
            var categoryPaths = (await connection.QueryAsync<string>(CategoryQueries.GetProductCategories, new { ProductId = productId })).ToList();

            var productDetail = mapper.Map<ProductCompleteDetailsDto>(product);
            //var productDetail = mapper.Map<ProductDetailsDto>(product);
            productDetail.Images = [.. images];
            productDetail.CategoryPaths = [.. categoryPaths];

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

    public async Task<ProductCompleteDetailsDto?> Update(ProductEditDto product)
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
}