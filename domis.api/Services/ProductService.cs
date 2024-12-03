using domis.api.Common;
using domis.api.DTOs.Common;
using domis.api.DTOs.Image;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductDetailsDto?> GetByIdWithDetails(int id, UserEntity? user);
    Task<ProductDetailsDto?> Update(ProductUpdateDto product);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
    Task<IEnumerable<SearchResultDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize);
    Task<bool> PutProductsOnSale(ProductSaleRequest request);
    Task<bool> AssignProductToCategory(AssignProductToCategoryRequest request);
}

public class ProductService(
    IProductRepository repository, 
    ICategoryRepository categoryRepo, 
    IUserRepository userRepo) : IProductService
{
    public async Task<IEnumerable<ProductPreviewDto>> GetAll()
        => await repository.GetAll();

    public async Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes()
        => await repository.GetAllQuantityTypes();

    public async Task<ProductDetailsDto?> GetByIdWithDetails(int id, UserEntity? user)
    {
        // var discount = await priceHelpers.GetDiscount(user);
        const int discount = 0;
        
        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id)
            : Roles.User.GetName();
        
        var productDetails = role == Roles.User.GetName() || role == Roles.Admin.GetName()
            ? await repository.GetByIdWithDetails(id, discount)
            : await repository.GetByIdWithDetailsForVp(id, role ?? Roles.User.GetName());

        return productDetails;
    }

    public async Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId)
        => await repository.GetProductsBasicInfoByCategory(categoryId);

    public async Task<ProductDetailsDto?> Update(ProductUpdateDto product)
        => await repository.Update(product);

    public async Task<IEnumerable<SearchResultDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize)
    {
        pageNumber ??= 1;
        pageSize ??= 20;
        
        return await repository.SearchProducts(searchTerm.ToLower(), pageNumber, pageSize);
    }

    public async Task<bool> PutProductsOnSale(ProductSaleRequest request)
    {
        if (request is { SalePrice: not null, SalePercentage: not null } or { SalePercentage: null, SalePrice: null} || request.SalePercentage is < 0 or > 100)
            throw new ArgumentException("Invalid sale percentage or price request.");
        
        return await repository.PutProductsOnSale(request);
    }

    public async Task<bool> AssignProductToCategory(AssignProductToCategoryRequest request)
    {
        var productExists = await repository.ProductExists(request.ProductId);
        if (!productExists) throw new ArgumentException("Product does not exist.");
        
        var categoryExists = await categoryRepo.CategoryExists(request.CategoryId);
        if (!categoryExists) throw new ArgumentException("Category does not exist.");
        
        return await repository.AssignProductToCategory(request);
    }
}