using System.Collections;
using domis.api.Common;
using domis.api.DTOs.Category;
using domis.api.DTOs.Product;
using domis.api.Endpoints.Helpers;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;
using PageOptions = domis.api.Models.PageOptions;

namespace domis.api.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryMenuDto>?> GetAll();
    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, UserEntity? user, ProductFilter? filters);
    Task<IEnumerable<ProductDetailsDto>> PutCategoryOnSale(CategorySaleRequest request);
}

public class CategoryService(ICategoryRepository repository, IProductRepository productRepo, IUserRepository userRepo) : ICategoryService
{
    public async Task<IEnumerable<CategoryMenuDto>?> GetAll()
    {
        return await repository.GetAll();
    }

    //probably no need for this one
    public async Task<Category?> GetById(int id)
    {
        return await repository.GetById(id);
    }

    public async Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, UserEntity? user, ProductFilter? filters)
    {
        //var discount = await priceHelpers.GetDiscount(user);
        const int discount = 0;
        
        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id) ?? Roles.User.GetName()
            : Roles.User.GetName();

        var categoryWithProducts = await repository.GetCategoryProducts(categoryId, options, filters, discount, role);

        //razmisliti o ovome -> trenutno: samo na prvoj stranici vracam max vrednosti za filtere
        if (options.PageNumber != 1) return categoryWithProducts;
        
        if (categoryWithProducts == null) return null;
        
        var maxFilterValues = await repository.GetProductsMaxFilterValues(categoryId);
        categoryWithProducts.MaxFilters = maxFilterValues;

        return categoryWithProducts;

    }

    public async Task<IEnumerable<ProductDetailsDto>> PutCategoryOnSale(CategorySaleRequest request)
    {
        if (request.SalePercentage is < 0 or > 100 || request is { SalePercentage: null })
            throw new ArgumentException("Sale percentage not valid.");
        
        if (!await repository.CategoryExists(request.CategoryId)) 
            throw new ArgumentException("Category does not exist.");
        
        var existingSales =  await repository.PutCategoryOnSale(request);

        var productsAlreadyOnSale = new List<ProductDetailsDto>();
        
        foreach (var sale in existingSales)
        {
            var product = await productRepo.GetByIdWithDetails(sale.ProductId, 0);
            if (product != null) productsAlreadyOnSale.Add(product);
        }
        
        return productsAlreadyOnSale;
    }
}