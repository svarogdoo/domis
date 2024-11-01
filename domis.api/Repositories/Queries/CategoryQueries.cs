namespace domis.api.Repositories.Helpers;

public static class CategoryQueries
{
    public const string GetAll = @"
        WITH RECURSIVE CategoryHierarchy AS (
            -- Anchor member: Start with top-level categories (categories with no parent)
            SELECT
                id AS Id,
                parent_category_id AS ParentCategoryId,
                category_name AS Name,
                sort_number AS SortNumber
            FROM domis.category
            WHERE parent_category_id IS NULL AND active = true
            
            UNION ALL

            -- Recursive member: Join to find subcategories
            SELECT
                c.id AS Id,
                c.parent_category_id AS ParentCategoryId,
                c.category_name AS Name,
                c.sort_number AS SortNumber
            FROM domis.category c
            INNER JOIN CategoryHierarchy ch
                ON c.parent_category_id = ch.Id
            WHERE c.active = true
        )

        -- Select all categories and their subcategories
        SELECT
            Id,
            ParentCategoryId,
            Name
        FROM CategoryHierarchy
        ORDER BY SortNumber ASC NULLS LAST, Name ASC; --sort by sort number, then by category name
    ";
    
    public static string GetProductCategories = @"
        WITH RECURSIVE RecursiveCategoryHierarchy AS (
            -- Anchor member: Start with categories for the product
            SELECT
                pc.product_id AS ProductId,
                c.id AS CategoryId,
                c.parent_category_id AS ParentCategoryId,
                c.id::text AS Path -- Start with the category ID as the path
            FROM domis.product_category pc
            JOIN domis.category c ON pc.category_id = c.id
            WHERE pc.product_id = @ProductId AND active = true

            UNION ALL

            -- Recursive member: Join to find parent categories
            SELECT
                rch.ProductId, -- Propagate product ID
                c.id AS CategoryId,
                c.parent_category_id AS ParentCategoryId,
                c.id::text || '/' || rch.Path AS Path -- Prepend the current category ID to the existing path
            FROM domis.category c
            INNER JOIN RecursiveCategoryHierarchy rch
                ON c.id = rch.ParentCategoryId
        )

        -- Select only the path and product ID for top-level categories (where ParentCategoryId is NULL)
        SELECT
            Path
        FROM RecursiveCategoryHierarchy
        WHERE ParentCategoryId IS NULL
        ORDER BY Path;";

    public const string GetCategoryById = @"
        SELECT id as Id, category_name as Name, category_description as Description 
        FROM domis.category
        WHERE id = @CategoryId;";
}
