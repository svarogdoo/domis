using domis.api.Services;

namespace domis.api.Endpoints;

public static class CategoryEndpoints
{
    public static void RegisterCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/categories").WithTags("Categories");

        group.MapGet("/", async (ICategoryService categoryService) =>
        {
            var response = await categoryService.GetAll();
            return Results.Ok(response);
        });

        group.MapGet("/{id:int}", async (int id, ICategoryService categoryService) =>
        {
            var product = await categoryService.GetById(id);

            return product is null ? Results.NotFound() : Results.Ok(product);
        });

    }
}
