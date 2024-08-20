using AutoMapper;
using Dapper;
using domis.api.Database;
using domis.api.DTOs;
using domis.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<ProductDetailDto2?> GetById(int id);
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

    public async Task<ProductDetailDto2?> GetById(int id)
    {
        const string sql = @"
            SELECT 
                p.id AS Id, 
                p.product_name AS Name, 
                p.product_description AS Description,
                p.sku AS Sku,
                p.price AS Price,
                p.stock AS Stock,
                p.active AS IsActive,
                i.image_url AS ImageUrl
            FROM domis.product p
            LEFT JOIN domis.product_image pi ON p.id = pi.product_id
            LEFT JOIN domis.image i ON pi.image_id = i.id
            WHERE p.id = @Id";

        var parameters = new { Id = id };

        var productDictionary = new Dictionary<int, ProductDetailDto2>();

        await connection.QueryAsync<Product, string, ProductDetailDto2>(
            sql,
            (product, imageUrl) =>
            {
                if (!productDictionary.TryGetValue(id, out var existingProduct))
                {
                    existingProduct = mapper.Map<ProductDetailDto2>(product);
                    productDictionary.Add(id, existingProduct);
                }

                // Add the image URL to the list
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    existingProduct.ImageUrls.Add(imageUrl);
                }

                return existingProduct;
            },
            splitOn: "ImageUrl",
            param: parameters
        );

        return productDictionary.Values.SingleOrDefault();
    }

    //public async Task<Product?> GetById2(int id)
    //{
    //    const string sql = @"
    //    SELECT 
    //        p.id AS Id, 
    //        p.product_name AS Name, 
    //        p.product_description AS Description,
    //        p.sku AS Sku,
    //        p.price AS Price,
    //        p.stock AS Stock,
    //        p.active AS IsActive,
    //        i.image_url AS ImageUrl
    //    FROM domis.product p
    //    LEFT JOIN domis.product_image pi ON p.id = pi.product_id
    //    LEFT JOIN domis.image i ON pi.image_id = i.id
    //    WHERE p.id = @Id";

    //    var parameters = new { Id = id };

    //    // Use a dictionary to handle potential multiple images
    //    var productDictionary = new Dictionary<int, Product>();

    //    await connection.QueryAsync<Product, string, Product>(
    //        sql,
    //        (product, imageUrl) =>
    //        {
    //            if (!productDictionary.TryGetValue((int)product.Id, out var existingProduct))
    //            {
    //                existingProduct = product;
    //                existingProduct.ImageUrls = new List<string>(); // Initialize list for image URLs
    //                productDictionary.Add((int)existingProduct.Id, existingProduct);
    //            }

    //            // Add the image URL to the list
    //            if (imageUrl != null && !string.IsNullOrEmpty(imageUrl))
    //            {
    //                existingProduct.ImageUrls.Add(imageUrl);
    //            }

    //            return existingProduct;
    //        },
    //        splitOn: "ImageUrl",
    //        param: parameters
    //    );

    //    // Return the product. If no product is found, return null.
    //    return productDictionary.Values.SingleOrDefault();
    //}



    public async Task<Product?> GetByIdSimple(int id)
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
                FROM domis.product
                WHERE id = @Id";

        var parameters = new { Id = id };

        var product = await connection.QuerySingleOrDefaultAsync<Product>(sql, parameters);

        return product;
    }

    public async Task<ProductDetailDto?> GetByIdWithCategoriesAndImages(int productId)
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
        productDetail.ImageUrls = imageUrls;
        productDetail.CategoryPaths = categoryPaths;

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
