using domis.api.Models;

namespace domis.api.Services;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetById(Guid id);
    void Add(Product product);
    void Update(Product product);
    void Delete(Guid id);
}

public class ProductService : IProductService
{
    private static readonly List<Product> _products;

    static ProductService()
    {
        _products =
        [
            new Product { Guid = Guid.NewGuid(), Name = "Laptop", Description = "A high-performance laptop for all your computing needs." },
            new Product { Guid = Guid.NewGuid(), Name = "Smartphone", Description = "A sleek and powerful smartphone with the latest features." },
            new Product { Guid = Guid.NewGuid(), Name = "Headphones", Description = "Noise-cancelling headphones with superior sound quality." },
            new Product { Guid = Guid.NewGuid(), Name = "Gaming Console", Description = "A next-gen gaming console for an immersive experience." },
            new Product { Guid = Guid.NewGuid(), Name = "Smartwatch", Description = "A stylish smartwatch with health tracking features." },
            new Product { Guid = Guid.NewGuid(), Name = "Camera", Description = "A high-resolution digital camera for professional photography." },
            new Product { Guid = Guid.NewGuid(), Name = "Tablet", Description = "A lightweight tablet with a sharp display and fast performance." },
            new Product { Guid = Guid.NewGuid(), Name = "Wireless Charger", Description = "A fast wireless charger compatible with most devices." },
            new Product { Guid = Guid.NewGuid(), Name = "Bluetooth Speaker", Description = "A portable Bluetooth speaker with deep bass and clear sound." },
            new Product { Guid = Guid.NewGuid(), Name = "Drone", Description = "A high-tech drone with a 4K camera and long battery life." }
        ];
    }

    public void Add(Product product)
    {
        _products.Add(product);
    }

    public void Delete(Guid id)
    {
        var product = GetById(id);
        if (product != null)
        {
            _products.Remove(product);
        }
    }

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product? GetById(Guid id)
    {
        return _products.FirstOrDefault(p => p.Guid == id);
    }

    public void Update(Product product)
    {
        var existingProduct = GetById(product.Guid);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
        }
    }
}
