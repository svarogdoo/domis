using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductDetailsDto?> GetByIdWithDetails(int id);
    Task<ProductDetailsDto?> Update(ProductUpdateDto product);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
    Task<IEnumerable<ProductBasicInfoDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize);
}

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
        => await repository.GetAll();

    public async Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes()
        => await repository.GetAllQuantityTypes();

    public async Task<ProductDetailsDto?> GetByIdWithDetails(int id)
        => await repository.GetByIdWithDetails(id);

    public async Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId)
        => await repository.GetProductsBasicInfoByCategory(categoryId);

    public async Task<ProductDetailsDto?> Update(ProductUpdateDto product)
        => await repository.Update(product);

    public async Task<IEnumerable<ProductBasicInfoDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize)
        => await repository.SearchProducts(searchTerm.ToLower(), pageNumber, pageSize);
}