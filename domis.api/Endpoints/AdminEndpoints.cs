using domis.api.Common;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Endpoints;

//TODO: uncomment RequireAuthorization
public static class AdminEndpoints
{
    public static void RegisterAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/admin").WithTags("Admin"); //RequireAuthorization(Roles.Admin.GetName());

        group.MapGet("/users", async (IAdminService adminService) =>
        {
            var users = await adminService.GetUsers();
            
            return Results.Ok(users);
        })
        .WithDescription("Get all users (id, username & role).");

        group.MapPost("/promote-to-admin", async ([FromBody] string userId, IAdminService adminService) =>
        {
            var userWithRoles = await adminService.PromoteToAdmin(userId);
            
            return userWithRoles is null
                ? Results.NotFound("User not found.")
                : Results.Ok("User promoted to admin successfully.");
        })
        .WithDescription("Promote user to admin role.");

        group.MapGet("/user/{userId}", async (string userId, IAdminService adminService) =>
        {
            var userWithRole = await adminService.GetUserById(userId);
            
            return userWithRole is not null
                ? Results.Ok(userWithRole)
                : Results.NotFound("User not found.");
        })
        .WithDescription("Get user by ID.");

        group.MapGet("/roles", async (IAdminService adminService) =>
        {
            var roles = await adminService.GetRoles();
            
            return Results.Ok(roles);
        })
        .WithDescription("Get all roles.");

        group.MapPut("/roles/discount", async ([FromBody] RoleDiscountRequest request, IAdminService adminService) =>
        {
            var updatedRole = await adminService.UpdateRoleDiscount(request.RoleName, request.Discount);
            
            return updatedRole is not null
                ? Results.Ok(updatedRole)
                : Results.BadRequest($"Failed to update role: {request.RoleName}.");
        })
        .WithDescription("Update role discount.");

        group.MapPut("/user-role/{userId}", async (string userId, [FromBody] RoleRequest request, IAdminService adminService) =>
        {
            if (!Enum.TryParse<Roles>(request.Role, true, out var role))
                return Results.BadRequest($"Role '{request.Role}' is not a valid role.");

            var success = await adminService.AddRoleToUser(userId, role);
            
            return success
                ? Results.Ok($"User promoted to {role.GetName()} successfully.")
                : Results.BadRequest($"Failed to promote user to {role.GetName()}.");

        })
        .WithDescription("Add user to a role.");

        group.MapDelete("/user-role/{userId}", async (string userId, [FromBody] RoleRequest request, IAdminService adminService) =>
        {
            if (!Enum.TryParse<Roles>(request.Role, true, out var role))
                return Results.BadRequest($"Role '{request.Role}' is not a valid role.");

            var success = await adminService.RemoveRoleFromUser(userId, role);
            
            return success
                ? Results.Ok($"User no longer has the {role.GetName()} role.")
                : Results.BadRequest($"Failed to remove {role.GetName()} role from user.");

        })
        .WithDescription("Remove user from a role.");

        group.MapGet("/orders", async (IAdminService adminService) =>
        {
            var response = await adminService.Orders();
            
            return Results.Ok(response);
        })
        .WithDescription("Gets all orders in the system.");

        group.MapPut("/orders/status", async ([FromBody] UpdateOrderStatusRequest request, IOrderService orderService) =>
        {
            var response = await orderService.UpdateOrderStatus(request.OrderId, request.StatusId);

            return Results.Ok(new UpdateOrderResponse(response));
        })
        .WithDescription("Update order status(1-New, 2-In Progress, 3-Sent, 4-Completed, 5-Canceled)");

        group.MapPost("/product/sale", async ([FromBody] ProductSaleRequest request, IProductService productService) =>
        {
            try
            {
                var success = await productService.PutProductsOnSale(request);
                
                return success
                    ? Results.Ok("Product put on sale successfully.")
                    : Results.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithDescription("Put a product on sale.");

        group.MapPost("/category/sale", async ([FromBody] CategorySaleRequest request, ICategoryService categoryService) =>
        {
            try
            {
                var result = await categoryService.PutCategoryOnSale(request);
                
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithDescription("Put products within category on sale. If some products are already on sale separately, " +
                         "they will not be put on sale and will be returned for admin to decide what to do.");

        group.MapDelete("/product/sale", async ([FromBody] List<int> request, IProductService productService) =>
        {
            var success = await productService.RemoveProductsFromSale(request);
            Results.Ok(success);
        })
        .WithDescription("Assign or update a category for a product.");

        group.MapPost("/product/category", async ([FromBody] AssignProductToCategoryRequest request, IProductService productService) =>
        {
            var success = await productService.AssignProductToCategory(request);
            
            return success
                ? Results.Ok("Product category updated successfully.")
                : Results.BadRequest("Failed to update product category.");
        })
        .WithDescription("Assign or update a category for a product.");

        group.MapPut("/products/{productId:int}/sizing", async (int productId, [FromBody] Size request, IProductService productService) =>
        {
            try
            {
                var updatedSize = await productService.UpdateProductSizing(productId, request);
                return Results.Ok(updatedSize);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        })
        .WithDescription("Update product sizing.");

        group.MapPut("/products/{productId:int}/pricing", async (int productId, [FromBody] ProductPriceUpdateDto request, IProductService productService) =>
        {
            try
            {
                var updatedPrice = await productService.UpdateProductPricing(productId, request);
                return Results.Ok(updatedPrice);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        })
        .WithDescription("Update product sizing.");

        group.MapGet("/product/{productId:int}/sale-history", async (int productId, IProductService productService) =>
        {
            try
            {
                var result = await productService.GetSaleHistory(productId);
                return Results.Ok(result);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        })
        .WithDescription("Gets history of product sale (reduced prices)");

        group.MapPost("/product", async ([FromBody] CreateProductInitialRequest request, IProductService productService) =>
        {
            try
            {
                var product = await productService.AddProduct(request);

                return product is null 
                    ? Results.StatusCode(500) 
                    : Results.Ok(product);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }).WithDescription("Add new product");
        
        //this one is not in use, *I think*
        group.MapPost("/product-2", async ([FromBody] CreateProductRequest request, IProductService productService) =>
        {
            try
            {
                var product = await productService.AddProduct(request);

                return product is null 
                    ? Results.StatusCode(500) 
                    : Results.Ok(product);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }).WithDescription("Add new product");
        
        group.MapPost("/users/{userId}/lock", async (string userId, UserManager<IdentityUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Results.NotFound("User not found");

            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            return Results.Ok("Korisnik zaključan.");
        })
        .WithDescription("Lock user");
        
        group.MapPost("/users/{userId}/unlock", async (string userId, UserManager<IdentityUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Results.NotFound("User not found");

            await userManager.SetLockoutEndDateAsync(user, null);

            return Results.Ok("Korisnik otključan.");
        })
        .WithDescription("Unlock user");

        group.MapPost("/images/{productId:int}/gallery-images", async (int productId, [FromBody] AddGalleryImagesRequest request, IImageService imageService) =>
        {
            try
            {
                var success = await imageService.AddGalleryImages(productId, request);

                return success
                    ? Results.Ok("Slike uspešno dodate.")
                    : Results.StatusCode(500);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        })
        .WithDescription("Add gallery images to product.");

        group.MapDelete("/images/{imageId:int}/products/{productId:int}", async (int imageId, int productId, IImageService imageService) =>
        {
            try
            {
                await imageService.DeleteGalleryImage(productId, imageId);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {                
                return Results.NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        })
        .WithDescription("Delete image from gallery.");
        
        
        group.MapPut("/images/{productId:int}/featured-image", async (int productId, [FromBody]string dataUrl, IImageService imageService) =>
        {
            try
            {
                var success = await imageService.UpdateFeaturedImage(productId, dataUrl);
            
                return success
                    ? Results.Ok("Slika uspešno promenjena.")
                    : Results.StatusCode(500);            
            }
            catch (NotFoundException ex)
            {                
                return Results.NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        })
        .WithDescription("Delete image from gallery.");
    }
}