using domis.api.Common;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductDetailsDto?> GetByIdWithDetails(int id, UserEntity? user);
    Task<ProductDetailsDto?> Update(ProductUpdateDto product);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
    Task<IEnumerable<ProductBasicInfoDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize);
}

public class ProductService(IProductRepository repository, IPriceHelpers priceHelpers) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
        => await repository.GetAll();

    public async Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes()
        => await repository.GetAllQuantityTypes();

    public async Task<ProductDetailsDto?> GetByIdWithDetails(int id, UserEntity? user)
    {
        var discount = await priceHelpers.GetDiscount(user);

        return await repository.GetByIdWithDetails(id, discount);
    }

    public async Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId)
        => await repository.GetProductsBasicInfoByCategory(categoryId);

    public async Task<ProductDetailsDto?> Update(ProductUpdateDto product)
        => await repository.Update(product);

    public async Task<IEnumerable<ProductBasicInfoDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize)
        => await repository.SearchProducts(searchTerm.ToLower(), pageNumber, pageSize);
}