using Dapper;
using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryPreviewDto>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryProductsDto?> GetCategoryProducts(int categoryId, Pagination pag);
}

public class CategoryRepository(IDbConnection connection) : ICategoryRepository
{
    public async Task<IEnumerable<CategoryPreviewDto>?> GetAll()
    {
        try
        {
            var categories = (await connection.QueryAsync<CategoryPreviewDto>(CategoryQueries.GetAll)).ToList();

            if (categories is null || categories.Count == 0)
            {
                return null;
            }

            var categoryDict = categories.ToDictionary(c => c.Id);
            foreach (var category in categories)
            {
                if (category.ParentCategoryId.HasValue && categoryDict.TryGetValue(category.ParentCategoryId.Value, out var parentCategory))
                {
                    parentCategory.Subcategories ??= [];
                    parentCategory.Subcategories.Add(category);
                }
            }

            // Return the top-level categories
            return categoryDict.Values.Where(c => !c.ParentCategoryId.HasValue).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while fetching categories"); throw;
        }
    }

    //probably no need for this one
    public async Task<Category?> GetById(int id)
    {
        var sql = @"";

        var parameters = new { Id = id };

        var product = await connection.QuerySingleOrDefaultAsync<Category>(sql, parameters);

        return product;
    }

    public async Task<CategoryProductsDto?> GetCategoryProducts(int categoryId, Pagination pag)
    {
        try
        {
            var offset = (pag.PageNumber - 1) * pag.PageSize;

            var categoryParams = new { CategoryId = categoryId };
            var category = await connection.QuerySingleOrDefaultAsync<CategoryBasic>(CategoryQueries.GetCategoryById, categoryParams);

            if (category is null)
                return null;

            offset = 0;  //TO-DO: remove when pagination is done on the FE

            var productParams = new { CategoryId = categoryId, Offset = offset, Limit = pag.PageSize };
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
}