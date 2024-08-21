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


        group.MapGet("/category/{categoryId:int}", async (int categoryId, int? page, int? size, IProductService productService) =>
        {
            var products = await productService.GetProductsByCategory(categoryId, page ?? 1, size ?? 20);

            return products is null ? Results.NoContent() : Results.Ok(products);
        }).WithDescription("get all products of a certain category");
    }
}