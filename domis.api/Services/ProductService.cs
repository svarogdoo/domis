using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetProducts();

    Task<ProductDetailDto?> GetSingle(int id);

    Task<IEnumerable<ProductPreviewDto>?> GetProductsByCategory(int categoryId, int pageNumber, int pageSize);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetProducts()
    {
        return await repository.GetAll();
    }

    public async Task<IEnumerable<ProductPreviewDto>?> GetProductsByCategory(int categoryId, int pageNumber, int pageSize)
    {
        return await repository.GetAllByCategory(categoryId, pageNumber, pageSize);
    }

    public async Task<ProductDetailDto?> GetSingle(int id)
    {
        return await repository.GetByIdWithCategoriesAndImagesSeparateQueries(id);
        //return await repository.GetByIdWithCategoriesAndImages(id);
    }
}