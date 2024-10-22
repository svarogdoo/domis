using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

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


        group.MapGet("/{id:int}", async (int id, IProductService productService, 
            HttpContext httpContext, UserManager<UserEntity> userManager) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);

            var product = await productService.GetByIdWithDetails(id, user);

            return product is null 
                ? Results.NotFound() 
                : Results.Ok(product);
        }).WithDescription("get product by id");


        group.MapPut("/", async (ProductUpdateDto product, IProductService productService) =>
        {
            try
            {
                var response = await productService.Update(product);

                return response is null
                    ? Results.NotFound(new { Message = $"Product with ID {product.Id} not found or update failed." })
                    : Results.Ok(response);
            }
            catch (PostgresException ex) when (ex.SqlState == "23503") // Foreign key violation
            {
                return Results.BadRequest(new { Message = "Invalid quantity type ID provided. Please check and try again." });
            }
            //TODO: whats this .Problem?
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
            
        }).WithDescription("update product");

        group.MapGet("/basic-info", async ([FromQuery]int categoryId, IProductService productService) =>
        {
            var products = await productService.GetProductsBasicInfoByCategory(categoryId);

            return products is null ? Results.NotFound() : Results.Ok(products);

        }).WithDescription("get products basic info by category");

        group.MapGet("/quantity-types", async (IProductService productService) =>
        {
            var quantityTypes = await productService.GetAllQuantityTypes();
            return Results.Ok(quantityTypes);
        }).WithDescription("Get all quantity types");

        group.MapGet("/search", async (string searchTerm, int? pageNumber, int? pageSize, IProductService productService) =>
        {
            if (searchTerm.Length < 3)
                return Results.Ok(new List<ProductBasicInfoDto>());

            var products = await productService.SearchProducts(searchTerm, pageNumber, pageSize);
            return Results.Ok(products);
        });
    }
}