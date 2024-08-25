using domis.api.DTOs.Product;
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
        }).WithDescription("get all products");


        group.MapGet("/{id:int}", async (int id, IProductService productService) =>
        {
            var product = await productService.GetByIdWithDetails(id);

            return product is null ? Results.NotFound() : Results.Ok(product);
        }).WithDescription("get product by id");


        group.MapPut("/", async (ProductEditDto product, IProductService productService) =>
        {
            var response = await productService.Update(product);

            return Results.NotFound();
            //return response ? Results.Ok() : Results.NotFound();
        }).WithDescription("update product");
    }
}