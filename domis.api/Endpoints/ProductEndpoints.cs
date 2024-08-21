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


        group.MapGet("/category/{categoryId:int}", async (int categoryId, int? page, int? size, IProductService productService) =>
        {
            var pageNumber = page.GetValueOrDefault(1);
            var pageSize = size.GetValueOrDefault(20);

            if (pageNumber <= 0)
            {
                return Results.BadRequest("Page number must be greater than 0.");
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                return Results.BadRequest("Page size must be between 1 and 100.");
            }

            var products = await productService.GetAllByCategory(categoryId, pageNumber, pageSize);

            return products is null ? Results.NoContent() : Results.Ok(products);
        }).WithDescription("get products by category");
    }
}