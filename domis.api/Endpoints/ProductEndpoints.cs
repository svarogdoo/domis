using domis.api.Services;

namespace domis.api.Endpoints;

public static class ProductEndpoints
{
    public static void RegisterProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/product").WithTags("Product");

        group.MapGet("/", async (IProductService productService) =>
        {
            var response = await productService.GetAll();
            return Results.Ok(response);
        });
    }
}
