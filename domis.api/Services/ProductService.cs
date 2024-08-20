using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll();
    Task<ProductDetailDto2?> GetById(int id);
    Task<ProductDetailDto?> GetByIdWithImagesAndCategories(int id);

    Task<IEnumerable<Product>?> GetAllByCategory(int categoryId);
}

public class ProductService(IProductRepository repository) : IProductService
{

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<IEnumerable<Product>?> GetAllByCategory(int categoryId)
    {
        return await repository.GetAllByCategory(categoryId);
    }

    public async Task<ProductDetailDto2?> GetById(int id)
    {
        return await repository.GetById(id);
    }

    public async Task<ProductDetailDto?> GetByIdWithImagesAndCategories(int id)
    {
        return await repository.GetByIdWithCategoriesAndImages(id);
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
