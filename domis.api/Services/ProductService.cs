using domis.api.Database;
using domis.api.Models;
using Microsoft.EntityFrameworkCore;

namespace domis.api.Services;

public interface IProductService
{
    Task<List<Product>> GetAll();
}

public class ProductService : IProductService
{
    private readonly DataContext _context;

    public ProductService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAll()
    {
        var products = await _context.Products.ToListAsync();

        return products;
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
