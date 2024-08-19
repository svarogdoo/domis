using Dapper;
using domis.api.Database;
using domis.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll();
}

public class ProductService : IProductService
{
    private readonly DataContext _context;
    private readonly IDbConnection _connection;

    public ProductService(DataContext context, IDbConnection connection)
    {
        _context = context;
        _connection = connection;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        var sql = @"
                    SELECT 
                        id AS Id, 
                        product_name AS Name, 
                        product_description AS Description,
                        sku AS Sku,
                        price AS Price,
                        stock AS Stock,
                        active AS IsActive
                    FROM domis.product";

        var products = await _connection.QueryAsync<Product>(sql);

        var productsEf = await _context.Products.ToListAsync();

        return products.ToList();
    }


    //public void Add(Product product)
    //{
    //    _products.Add(product);
    //}

    //public void Delete(Guid id)
    //{
    //    var product = GetById(id);
    //    if (product != null)
    //    {
    //        _products.Remove(product);
    //    }
    //}

    //public IEnumerable<Product> GetAll()
    //{
    //    return _products;
    //}

    //public Product? GetById(Guid id)
    //{
    //    return _products.FirstOrDefault(p => p.Guid == id);
    //}

    //public void Update(Product product)
    //{
    //    var existingProduct = GetById(product.Guid);
    //    if (existingProduct != null)
    //    {
    //        existingProduct.Name = product.Name;
    //        existingProduct.Description = product.Description;
    //    }
    //}
}
