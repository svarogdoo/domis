using domis.api.Common;
using domis.api.DTOs.Category;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryMenuDto>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
    Task<CategoryWithProductsDto?> GetCategoryProducts(int categoryId, PageOptions options, UserEntity? user);
}

public class CategoryService(ICategoryRepository repository, IPriceHelpers priceHelpers) : ICategoryService
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

        return await repository.GetCategoryProducts(categoryId, options, discount);
    }
}