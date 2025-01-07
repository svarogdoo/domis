using domis.api.Endpoints.Helpers;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Models.Enums;
using domis.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PageOptions = domis.api.Endpoints.Helpers.PageOptions;

namespace domis.api.Endpoints;

public static class CategoryEndpoints
{
    public static void RegisterCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/categories").WithTags("Categories");


        group.MapGet("/", async (ICategoryService categoryService) =>
        {
            var categories = await categoryService.GetAll();

            return categories is null ? Results.NotFound() : Results.Ok(categories);

        }).WithDescription("get all categories");


        //maybe no need for this one ?
        group.MapGet("/{id:int}", async (int id, ICategoryService categoryService) =>
        {
            var product = await categoryService.GetById(id);

            return product is null ? Results.NotFound() : Results.Ok(product);

        }).WithDescription("get category by id | NOT IMPLEMENTED");


        group.MapGet("/{categoryId:int}/products", async (int categoryId, [AsParameters]PageOptions options, [AsParameters]ProductFilter filters, 
            ICategoryService categoryService, HttpContext httpContext, UserManager<UserEntity> userManager) =>
        {
            if (options.PageNumber <= 0)
                return Results.BadRequest("Page number must be greater than 0.");

            if (options.PageSize is <= 0 or > 500)
                return Results.BadRequest("Page size must be between 1 and 500.");

            var pageOptions = new Models.PageOptions 
            { 
                PageNumber = options.PageNumber ?? 1, 
                PageSize = options.PageSize ?? 18,
                Sort = options.Sort ?? SortProductEnum.NameAsc
            };

            var user = await userManager.GetUserAsync(httpContext.User);

            var categoryWithProducts = await categoryService.GetCategoryProducts(categoryId, pageOptions, user, filters);

            return categoryWithProducts is null 
                ? Results.NotFound() 
                : Results.Ok(categoryWithProducts);

        }).WithDescription("get products by category");
        
        group.MapGet("sale/products", async (IProductService productService) =>
        {
            var products = await productService.GetProductsOnSaleAsync();
            return Results.Ok(new { products });
        }).WithDescription("get products that are on sale");
    }
}