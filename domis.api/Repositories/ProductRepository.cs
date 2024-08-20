using AutoMapper;
using Dapper;
using domis.api.DTOs;
using domis.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();

    Task<ProductDetailDto?> GetByIdWithCategoriesAndImages(int id);

    Task<IEnumerable<Product>?> GetAllByCategory(int categoryId);

    Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records);
}

public class ProductRepository(IDbConnection connection, IMapper mapper/*, DataContext context*/) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAll()
    {
        const string sql = @"
                    SELECT
                        id AS Id,
                        product_name AS Name,
                        product_description AS Description,
                        sku AS Sku,
                        price AS Price,
                        stock AS Stock,
                        active AS IsActive
                    FROM domis.product";

        var products = await connection.QueryAsync<Product>(sql);
        //var productsEF = await context.Products.ToListAsync();

        return products;
    }

    public async Task<IEnumerable<Product>?> GetAllByCategory(int categoryId)
    {
        const string sql = @"
            WITH RECURSIVE CategoryHierarchy AS (
                -- Anchor member: Start with the given category
                SELECT id
                FROM domis.category
                WHERE id = @CategoryId

                UNION ALL

                -- Recursive member: Join to get all subcategories
                SELECT c.id
                FROM domis.category c
                INNER JOIN CategoryHierarchy ch ON c.parent_category_id = ch.id
            )

            SELECT p.id AS Id,
                   p.product_name AS Name,
                   p.product_description AS Description,
                   p.sku AS Sku,
                   p.price AS Price,
                   p.stock AS Stock,
                   p.active AS IsActive
            FROM domis.product p
            INNER JOIN domis.product_category pc ON p.id = pc.product_id
            INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id
        ";

        var parameters = new { CategoryId = categoryId };
        return await connection.QueryAsync<Product>(sql, parameters);
    }

    public async Task<ProductDetailDto?> GetByIdWithCategoriesAndImages(int productId)
    {
        const string sql = @"
        WITH RECURSIVE RecursiveCategoryHierarchy AS (
            -- Anchor member: Start with categories for the product
            SELECT
                pc.product_id AS ProductId,
                c.id AS CategoryId,
                c.parent_category_id AS ParentCategoryId,
                c.id::text AS Path -- Start with the category ID as the path
            FROM domis.product_category pc
            JOIN domis.category c ON pc.category_id = c.id
            WHERE pc.product_id = @ProductId

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
        ),
        ProductInfo AS (
            SELECT
                id AS Id,
                product_name AS Name,
                product_description AS Description,
                sku AS Sku,
                price AS Price,
                stock AS Stock,
                active AS IsActive
            FROM domis.product
            WHERE id = @ProductId
        ),
        ProductImages AS (
            SELECT
                pi.product_id AS ProductId,
                i.image_url AS ImageUrl
            FROM domis.product_image pi
            JOIN domis.image i ON pi.image_id = i.id
            WHERE pi.product_id = @ProductId
        )
        SELECT
            p.Id,
            p.Name,
            p.Description,
            p.Sku,
            p.Price,
            p.Stock,
            p.IsActive,
            ARRAY_AGG(DISTINCT pi.ImageUrl) AS ImageUrls,
            ARRAY_AGG(DISTINCT rch.Path) AS CategoryPaths
        FROM ProductInfo p
        LEFT JOIN ProductImages pi ON p.Id = pi.ProductId
        LEFT JOIN RecursiveCategoryHierarchy rch ON p.Id = rch.ProductId
        WHERE rch.ParentCategoryId IS NULL
        GROUP BY p.Id, p.Name, p.Description, p.Sku, p.Price, p.Stock, p.IsActive;";

        var result = await connection.QuerySingleOrDefaultAsync<ProductDetailDto>(sql, new { ProductId = productId });

        if (result == null)
            return null;

        return result;
    }

    [Obsolete("Using separate dapper calls. Slower than GetByIdWithCategoriesAndImages")]
    public async Task<ProductDetailDto?> GetByIdWithCategoriesAndImagesSeparateQueries(int productId)
    {
        var productQuery = @"
            SELECT
                id AS Id,
                product_name AS Name,
                product_description AS Description,
                sku AS Sku,
                price AS Price,
                stock AS Stock,
                active AS IsActive
            FROM domis.product
            WHERE id = @ProductId;";

        var imagesQuery = @"
            SELECT
                i.image_url AS ImageUrl
            FROM domis.product_image pi
            JOIN domis.image i ON pi.image_id = i.id
            WHERE pi.product_id = @ProductId;";

        var categoriesQuery = @"
            WITH RECURSIVE RecursiveCategoryHierarchy AS (
                -- Anchor member: Start with categories for the product
                SELECT
                    pc.product_id AS ProductId,
                    c.id AS CategoryId,
                    c.parent_category_id AS ParentCategoryId,
                    c.id::text AS Path -- Start with the category ID as the path
                FROM domis.product_category pc
                JOIN domis.category c ON pc.category_id = c.id
                WHERE pc.product_id = @ProductId

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

        var product = await connection.QuerySingleOrDefaultAsync<Product>(productQuery, new { ProductId = productId });
        if (product == null)
            return null;

        var imageUrls = (await connection.QueryAsync<string>(imagesQuery, new { ProductId = productId })).ToList();
        var categoryPaths = (await connection.QueryAsync<string>(categoriesQuery, new { ProductId = productId })).ToList();

        var productDetail = mapper.Map<ProductDetailDto>(product);
        productDetail.ImageUrls = [.. imageUrls];
        productDetail.CategoryPaths = [.. categoryPaths];

        return productDetail;
    }

    public async Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records)
    {
        const string sql = @"
            UPDATE domis.product
            SET price = CASE
                WHEN sku = @Sku THEN @Price
            END,
            stock = CASE
                WHEN sku = @Sku THEN @Stock
            END
            WHERE sku = @Sku";

        var result = await connection.ExecuteAsync(sql, records);

        return result > 0;
    }
}