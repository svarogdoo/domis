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
}

public class CategoryRepository(IDbConnection connection) : ICategoryRepository
{
    public async Task<IEnumerable<CategoryPreviewDto>?> GetAll()
    {
        const string sql = CategoryQueries.GetAll;

        try
        {
            var categories = (await connection.QueryAsync<CategoryPreviewDto>(sql)).ToList();

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
}