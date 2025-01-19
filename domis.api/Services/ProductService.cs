using domis.api.Common;
using domis.api.DTOs.Common;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IProductService
{
    Task<ProductDetailsDto?> AddProduct(CreateProductRequest newProduct);
    Task<ProductDetailsDto?> AddProduct(CreateProductInitialRequest newProduct);
    Task<IEnumerable<ProductPreviewDto>> GetAll();
    Task<ProductDetailsDto?> GetByIdWithDetails(int id, UserEntity? user);
    Task<ProductDetailsDto?> Update(ProductUpdateDto product);
    Task<IEnumerable<ProductBasicInfoDto>> GetProductsBasicInfoByCategory(int categoryId);
    Task<IEnumerable<ProductQuantityTypeDto>> GetAllQuantityTypes();
    Task<IEnumerable<SearchResultDto>> SearchProducts(string searchTerm, int? pageNumber, int? pageSize);
    Task<bool> PutProductsOnSale(ProductSaleRequest request);
    Task<bool> RemoveProductsFromSale(List<int> productIds);
    Task<bool> AssignProductToCategory(AssignProductToCategoryRequest request);
    Task<IEnumerable<ProductPreviewDto>> GetProductsOnSaleAsync();
    Task<Size?> UpdateProductSizing(int productId, Size updatedSizing);
    Task<bool> UpdateProductPricing(int productId, ProductPriceUpdateDto updatedPricing);
    Task<IEnumerable<ProductSaleHistoryDto>> GetSaleHistory(int productId);
}

public class ProductService(
    IProductRepository repository, 
    ICategoryRepository categoryRepo, 
    IUserRepository userRepo) : IProductService
{
    public async Task<ProductDetailsDto?> AddProduct(CreateProductRequest newProduct)
    {
        var productId = await repository.CreateProduct(newProduct);

        if (productId < 1) return null;
        
        var res1 = await AssignProductToCategory(new AssignProductToCategoryRequest(productId, newProduct.CategoryId, false));

        if (!res1) return null;
        
        if (newProduct.Price is not null) //TODO: check once implemented on FE
            await UpdateProductPricing(productId, newProduct.Price);
        
        if (newProduct.Size is not null) //TODO: check once implemented on FE
            await UpdateProductSizing(productId, newProduct.Size);

        var productResult = await GetByIdWithDetails(productId, null);
        
        return productResult;
    }

    public async Task<ProductDetailsDto?> AddProduct(CreateProductInitialRequest newProduct)
    {
        var categoryExists = await categoryRepo.CategoryExists(newProduct.CategoryId);
        if (!categoryExists) 
            throw new ArgumentException("Izabrana kategorija ne postoji.");
        
        var skuExists = await repository.CheckIfSkuExists(newProduct.Sku);
        if (skuExists)
            throw new ArgumentException("Proizvod sa ovim SKU već postoji.");

        var productId = await repository.CreateProduct(newProduct);
        if (productId < 1) throw new Exception("Error occurred while creating product.");
        
        var res1 = await AssignProductToCategory(new AssignProductToCategoryRequest(productId, newProduct.CategoryId, false));
        if (!res1) return null;
        
        var productResult = await GetByIdWithDetails(productId, null);

        return productResult;
    }

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

    public async Task<bool> RemoveProductsFromSale(List<int> productIds)
    {
        if (productIds.Count == 0)
            return false;
        
        return await repository.RemoveProductsFromSale(productIds);
    }

    public async Task<bool> AssignProductToCategory(AssignProductToCategoryRequest request)
    {
        var productExists = await repository.ProductExists(request.ProductId);
        if (!productExists) throw new ArgumentException("Product has not been created.");
        
        return await repository.AssignProductToCategory(request);
    }

    public async Task<IEnumerable<ProductPreviewDto>> GetProductsOnSaleAsync() 
        => await repository.GetProductsOnSaleAsync();

    public async Task<Size?> UpdateProductSizing(int productId, Size updatedSizing)
    {
        var exists = await repository.ProductExists(productId);
        if (!exists)
            throw new NotFoundException("Product does not exist.");
        
        return await repository.UpdateProductSizing(productId, updatedSizing);
    }

    public async Task<bool> UpdateProductPricing(int productId, ProductPriceUpdateDto updatedPricing)
    {
        var exists = await repository.ProductExists(productId);
        if (!exists)
            throw new NotFoundException("Product does not exist.");

        return await repository.UpdateProductPricing(productId, updatedPricing);
    }

    public async Task<IEnumerable<ProductSaleHistoryDto>> GetSaleHistory(int productId)
    {
        var exists = await repository.ProductExists(productId);
        if (!exists)
            throw new NotFoundException("Product does not exist.");
        
        return await repository.GetSaleHistory(productId);
    }
}