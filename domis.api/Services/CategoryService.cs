using System.Collections;
using domis.api.Common;
using domis.api.DTOs.Category;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryMenuDto>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, UserEntity? user);
    Task<IEnumerable<ProductDetailsDto>> PutCategoryOnSale(CategorySaleRequest request);
}

public class CategoryService(ICategoryRepository repository, IProductRepository productRepo, IPriceHelpers priceHelpers, IUserRepository userRepo) : ICategoryService
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

    public async Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, UserEntity? user)
    {
        var discount = await priceHelpers.GetDiscount(user);
        
        var role = user is not null
            ? await userRepo.GetUserRoleAsync(user.Id)
            : Roles.User.GetName();
        
        return await repository.GetCategoryProducts(categoryId, options, discount, role ?? Roles.User.GetName());
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