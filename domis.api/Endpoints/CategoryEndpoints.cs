using domis.api.Models;
using domis.api.Services;

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


        //probably no need for this one
        group.MapGet("/{id:int}", async (int id, ICategoryService categoryService) =>
        {
            var product = await categoryService.GetById(id);

            return product is null ? Results.NotFound() : Results.Ok(product);
        }).WithDescription("get category by id | NOT IMPLEMENTED");

    }
}
