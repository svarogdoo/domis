using AutoMapper;
using Dapper;
using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using domis.api.Repositories.Queries;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();

    Task<ProductDetailDto?> GetByIdWithDetails(int id);

    Task<CategoryProductsDto?> GetAllByCategory(int categoryId, int pageNumber, int pageSize);

    Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records);
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

    public async Task<CategoryProductsDto?> GetAllByCategory(int categoryId, int pageNumber, int pageSize)
    {
        try
        {
            var offset = (pageNumber - 1) * pageSize;

            var categoryParams = new { CategoryId = categoryId };
            var category = await connection.QuerySingleOrDefaultAsync<CategoryBasic>(CategoryQueries.GetCategoryById, categoryParams);

            if (category is null)
                return null;

            var productParams = new { CategoryId = categoryId/*, Offset = offset, Limit = pageSize*/ }; //TO-DO: include when pagination is done on the FE
            var products = await connection.QueryAsync<ProductPreviewDto>(ProductQueries.GetAllByCategory, productParams);

            var result = new CategoryProductsDto
            {
                Category = category,
                Products = products.ToList()
            };

            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error ocurred while getting products by category"); throw;
        }
    }

    public async Task<ProductDetailDto?> GetByIdWithDetails(int productId)
    {
        try
        {
            var product = await connection.QuerySingleOrDefaultAsync<Product>(ProductQueries.GetById, new { ProductId = productId });
            if (product == null)
                return null;

            var images = (await connection.QueryAsync<ImageDto>(ImageQueries.GetProductImages, new { ProductId = productId })).ToList();
            var categoryPaths = (await connection.QueryAsync<string>(CategoryQueries.GetProductCategories, new { ProductId = productId })).ToList();

            var productDetail = mapper.Map<ProductDetailDto>(product);
            productDetail.Images = [.. images];
            productDetail.CategoryPaths = [.. categoryPaths];

            return productDetail;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching product details"); throw;
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
}