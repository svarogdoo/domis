using domis.api.Models;
using domis.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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


        group.MapGet("/{categoryId}/products", async (int categoryId, int? pageNumber, int? pageSize, ICategoryService categoryService, HttpContext httpContext, UserManager<UserEntity> userManager) =>
        {
            if (pageNumber <= 0)
            {
                return Results.BadRequest("Page number must be greater than 0.");
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                return Results.BadRequest("Page size must be between 1 and 100.");
            }

            var pagination = new PageOptions 
            { 
                PageNumber = pageNumber ?? 1, 
                PageSize = pageSize ?? 20 
            };

            var user = await userManager.GetUserAsync(httpContext.User);

            var categories = await categoryService.GetCategoryProducts(categoryId, pagination, user);

            return categories is null ? Results.NotFound() : Results.Ok(categories);

        }).WithDescription("get products by category");
    }
}