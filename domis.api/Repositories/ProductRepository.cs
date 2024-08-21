using AutoMapper;
using Dapper;
using domis.api.DTOs;
using domis.api.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();

    Task<ProductDetailDto?> GetByIdWithCategoriesAndImages(int id);
    Task<ProductDetailDto?> GetByIdWithCategoriesAndImagesSeparateQueries(int id);

    Task<IEnumerable<ProductPreviewDto>?> GetAllByCategory(int categoryId, int pageNumber, int pageSize);

    Task<bool> NivelacijaUpdateProductBatch(IEnumerable<NivelacijaRecord> records);
}

public class ProductRepository(IDbConnection connection, IMapper mapper/*, DataContext context*/) : IProductRepository
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
    {
        //TO-DO: check if we need this, and if we do, check if we want to include Featured image as well
        const string sql = @"
                    SELECT
                        product_name AS Name,
                        sku AS Sku,
                        price AS Price,
                        stock AS Stock
                    FROM domis.product";

        var products = await connection.QueryAsync<ProductPreviewDto>(sql);
        //var productsEF = await context.Products.ToListAsync();

        return products;
    }

    public async Task<IEnumerable<ProductPreviewDto>?> GetAllByCategory(int categoryId, int pageNumber, int pageSize)
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
            ),
            ProductImages AS (
                -- Get Featured image for each product
                SELECT
                    pi.product_id AS ProductId,
                    i.image_url AS FeaturedImageUrl
                FROM domis.product_image pi
                JOIN domis.image i ON pi.image_id = i.id
                JOIN domis.image_type it ON pi.image_type_id = it.id
                WHERE it.image_type_name = 'Featured'
            ),
            ProductsWithImages AS (
                -- Combine products with their Featured images
                SELECT
                    p.product_name AS Name,
                    p.sku AS Sku,
                    p.price AS Price,
                    p.stock AS Stock,
                    p.active AS IsActive,
                    pi.FeaturedImageUrl
                FROM domis.product p
                INNER JOIN domis.product_category pc ON p.id = pc.product_id
                INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id
                LEFT JOIN ProductImages pi ON p.id = pi.ProductId
                WHERE p.active = true -- filter to include only active products
            )
            SELECT *
            FROM ProductsWithImages
            ORDER BY Name -- Ensure you have a column to order by
            OFFSET @Offset LIMIT @Limit;
        ";

        try
        {
            var offset = (pageNumber - 1) * pageSize;

            var parameters = new { CategoryId = categoryId, Offset = offset, Limit = pageSize };
            var products = await connection.QueryAsync<ProductPreviewDto>(sql, parameters);

            return products.ToList();
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred: {exception}", ex);
            return null;
        }
    }

    public async Task<ProductDetailDto?> GetByIdWithCategoriesAndImages(int productId)
    {

        const string sql = @"";

        // Execute the query
        var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { ProductId = productId });

        if (result == null)
            return null;

        // Map the result to ProductDetailDto
        var productDetail = new ProductDetailDto
        {
            Name = result.Name,
            Description = result.Description,
            Sku = result.Sku,
            Price = result.Price,
            Stock = result.Stock,
            IsActive = result.IsActive,
            CategoryPaths = ((IEnumerable<object>)result.CategoryPaths).Cast<string>().ToArray(),
            Images = ((IEnumerable<dynamic>)result.Images)
                .Select(img => new ImageDto
                {
                    Url = (string)img.Url,
                    Type = (string)img.Type
                })
                .ToList()
        };

        var product = await connection.QuerySingleOrDefaultAsync<ProductDetailDto>(sql, new { ProductId = productId });

        if (product == null)
            return null;

        return product;
    }

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
                i.image_url AS Url,
                it.image_type_name AS Type
            FROM domis.product_image pi
            JOIN domis.image i ON pi.image_id = i.id
            JOIN domis.image_type it ON pi.image_type_id = it.id
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

        var images = (await connection.QueryAsync<ImageDto>(imagesQuery, new { ProductId = productId })).ToList();
        var categoryPaths = (await connection.QueryAsync<string>(categoriesQuery, new { ProductId = productId })).ToList();

        var productDetail = mapper.Map<ProductDetailDto>(product);
        productDetail.Images = [.. images];
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