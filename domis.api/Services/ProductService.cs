using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();

    Task<ProductDetailDto?> GetByIdWithDetails(int id);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<ProductDetailDto?> GetByIdWithDetails(int id)
    {
        return await repository.GetByIdWithDetails(id);
        //return await repository.GetByIdWithCategoriesAndImages(id);
    }
}