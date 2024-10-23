using Dapper;
using domis.api.Common;
using domis.api.DTOs.Category;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories.Helpers;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryMenuDto>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, decimal discount);
}

public class CategoryRepository(IDbConnection connection) : ICategoryRepository
{
    public async Task<IEnumerable<CategoryMenuDto>?> GetAll()
    {
        try
        {
            var categories = (await connection.QueryAsync<CategoryMenuDto>(CategoryQueries.GetAll)).ToList();

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

    public async Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, decimal discount = 0)
    {
        try
        {
            var offset = (options.PageNumber - 1) * options.PageSize;

            var categoryParams = new { CategoryId = categoryId };
            var category = await connection.QuerySingleOrDefaultAsync<CategoryBasicInfoDto>(CategoryQueries.GetCategoryById, categoryParams);

            if (category is null)
                return null;

            //offset = 0; options.PageSize = 1000;  //TODO: remove when pagination is done on the FE

            var productParams = new { CategoryId = categoryId, Offset = offset, Limit = options.PageSize };
            var products = await connection.QueryAsync<ProductPreviewDto>(ProductQueries.GetAllByCategoryWithPagination, productParams);

            var result = new CategoryWithProductsDto
            {
                Category = category,
                Products = products.Select(product => new ProductPreviewDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = PricingHelper.CalculateDiscount(product.Price, discount)
                }).ToList()
            };


            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error ocurred while getting products by category"); throw;
        }
    }
}