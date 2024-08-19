using domis.api.Services;

namespace domis.api.Endpoints;

public static class ProductEndpoints
{
    public static void RegisterProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (IProductService productService) =>
        {
            var response = await productService.GetAll();
            return Results.Ok(response);
        });

        group.MapGet("/{id:int}", async (int id, IProductService productService) =>
        {
            var product = await productService.GetById(id);
             
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

    }
}
