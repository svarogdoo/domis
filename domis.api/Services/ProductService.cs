using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();

    Task<ProductCompleteDetailsDto?> GetByIdWithDetails(int id);
    Task<ProductCompleteDetailsDto?> Update(ProductEditDto product);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<ProductCompleteDetailsDto?> GetByIdWithDetails(int id)
    {
        return await repository.GetByIdWithDetails(id);
    }

    public async Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId)
        => await repository.GetProductsBasicInfoByCategory(categoryId);

    public async Task<ProductCompleteDetailsDto?> Update(ProductEditDto product)
    {
        return await repository.Update(product);
    }
}