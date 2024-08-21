using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();

    Task<ProductDetailDto?> GetByIdWithDetails(int id);

    Task<IEnumerable<ProductPreviewDto>?> GetAllByCategory(int categoryId, int pageNumber, int pageSize);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<IEnumerable<ProductPreviewDto>?> GetAllByCategory(int categoryId, int pageNumber, int pageSize)
    {
        return await repository.GetAllByCategory(categoryId, pageNumber, pageSize);
    }

    public async Task<ProductDetailDto?> GetByIdWithDetails(int id)
    {
        return await repository.GetByIdWithDetails(id);
        //return await repository.GetByIdWithCategoriesAndImages(id);
    }
}