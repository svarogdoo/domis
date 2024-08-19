using Dapper;
using domis.api.Database;
using domis.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace domis.api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<IEnumerable<Product>?> GetAllByCategory(int categoryId);
    Task<bool> NivelacijaUpdateProduct(NivelacijaRecord updatedRecord);

}

public class ProductRepository(IDbConnection connection/*, DataContext context*/) : IProductRepository
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

    public async Task<Product?> GetById(int id)
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

    public async Task<bool> NivelacijaUpdateProduct(NivelacijaRecord updatedRecord)
    {
        const string sql = @"
            UPDATE domis.product
            SET price = @Price,
                stock = @Stock
            WHERE id = @ProductId";

        //TO-DO: uncomment when ready
        //var result = await connection.ExecuteAsync(sql, new { updatedRecord.Id, updatedRecord.Price, updatedRecord.Stock });
        //return result > 0;

        return false;
    }
}
