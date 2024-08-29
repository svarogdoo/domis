using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();

    Task<ProductDetailsDto?> GetByIdWithDetails(int id);
    Task<ProductUpdateResponseDto?> Update(ProductEditDto product);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<ProductDetailsDto?> GetByIdWithDetails(int id)
    {
        return await repository.GetByIdWithDetails(id);
    }

    public async Task<ProductUpdateResponseDto?> Update(ProductEditDto product)
    {
        return await repository.Update(product);
    }
}