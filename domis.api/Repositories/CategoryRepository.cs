using Dapper;
using domis.api.DTOs;
using domis.api.Models;
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
        const string sql = @"
            WITH RECURSIVE CategoryHierarchy AS (
                -- Anchor member: Start with top-level categories (categories with no parent)
                SELECT
                    id AS Id,
                    parent_category_id AS ParentCategoryId,
                    category_name AS Name
                FROM domis.category
                WHERE parent_category_id IS NULL

                UNION ALL

                -- Recursive member: Join to find subcategories
                SELECT
                    c.id AS Id,
                    c.parent_category_id AS ParentCategoryId,
                    c.category_name AS Name
                FROM domis.category c
                INNER JOIN CategoryHierarchy ch
                    ON c.parent_category_id = ch.Id
            )

            -- Select all categories and their subcategories
            SELECT
                Id,
                ParentCategoryId,
                Name
            FROM CategoryHierarchy
            ORDER BY Id;
        ";

        var categories = (await connection.QueryAsync<CategoryPreviewDto>(sql)).ToList();

        if (categories is null || categories.Count == 0)
        {
            return null;
        }

        // Build the hierarchical structure
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

    //probably no need for this one
    public async Task<Category?> GetById(int id)
    {
        var sql = @"";

        var parameters = new { Id = id };

        var product = await connection.QuerySingleOrDefaultAsync<Category>(sql, parameters);

        return product;
    }
}