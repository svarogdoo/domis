using domis.api.DTOs;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll();

    Task<ProductDetailDto?> GetByIdWithImagesAndCategories(int id);

    Task<IEnumerable<ProductPreviewDto>?> GetAllByCategory(int categoryId);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<Product>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<IEnumerable<ProductPreviewDto>?> GetAllByCategory(int categoryId)
    {
        return await repository.GetAllByCategory(categoryId);
    }

    public async Task<ProductDetailDto?> GetByIdWithImagesAndCategories(int id)
    {
        return await repository.GetByIdWithCategoriesAndImagesSeparateQueries(id);

        //return await repository.GetByIdWithCategoriesAndImages(id);
    }
}