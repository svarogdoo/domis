using domis.api.Services;

namespace domis.api.Endpoints;

public static class ProductEndpoints
{
    public static void RegisterProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products").WithTags("Products");


        group.MapGet("/", async (IProductService productService) =>
        {
            var response = await productService.GetProducts();

            return Results.Ok(response);
        }).WithDescription("get all products");


        group.MapGet("/{id:int}", async (int id, IProductService productService) =>
        {
            var product = await productService.GetSingle(id);

            return product is null ? Results.NotFound() : Results.Ok(product);
        }).WithDescription("get product by id");


        group.MapGet("/category/{categoryId:int}", async (int categoryId, IProductService productService) =>
        {
            var product = await productService.GetProductsByCategory(categoryId);

            return product is null ? Results.NoContent() : Results.Ok(product);
        }).WithDescription("get all products of a certain category");
    }
}