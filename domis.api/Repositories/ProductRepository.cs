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
}
