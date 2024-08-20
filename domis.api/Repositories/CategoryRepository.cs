using Dapper;
using domis.api.Models;
using System.Data;

namespace domis.api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>?> GetAll();
    Task<Category?> GetById(int id);
}

public class CategoryRepository(IDbConnection connection) : ICategoryRepository
{
    public async Task<IEnumerable<Category>?> GetAll()
    {
        const string sql = @"
            WITH RECURSIVE CategoryHierarchy AS (
                -- Anchor member: Start with top-level categories (categories with no parent)
                SELECT 
                    id AS CategoryId,
                    parent_category_id AS ParentCategoryId,
                    category_name AS CategoryName,
                    category_description AS CategoryDescription
                FROM domis.category
                WHERE parent_category_id IS NULL
                
                UNION ALL
                
                -- Recursive member: Join to find subcategories
                SELECT 
                    c.id AS CategoryId,
                    c.parent_category_id AS ParentCategoryId,
                    c.category_name AS CategoryName,
                    c.category_description AS CategoryDescription
                FROM domis.category c
                INNER JOIN CategoryHierarchy ch
                    ON c.parent_category_id = ch.CategoryId
            )
            
            -- Select all categories and their subcategories
            SELECT 
                CategoryId, 
                ParentCategoryId, 
                CategoryName, 
                CategoryDescription
            FROM CategoryHierarchy
            ORDER BY CategoryId;
        ";

        var categories = (await connection.QueryAsync<Category>(sql)).ToList();

        if (categories is null)
        {
            return null;
        }

        // Build the hierarchical structure
        var categoryDict = categories.ToDictionary(c => c.CategoryId);
        foreach (var category in categories)
        {
            if (category.ParentCategoryId.HasValue && categoryDict.TryGetValue(category.ParentCategoryId.Value, out var parentCategory))
            {
                parentCategory.Subcategories.Add(category);
            }
        }

        // Return the top-level categories
        return categoryDict.Values.Where(c => !c.ParentCategoryId.HasValue).ToList();
    }

    public async Task<Category?> GetById(int id)
    {
        var sql = @"";

        var parameters = new { Id = id };

        var product = await connection.QuerySingleOrDefaultAsync<Category>(sql, parameters);

        return product;
    }
}
