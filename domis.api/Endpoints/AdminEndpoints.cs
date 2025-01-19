using domis.api.Common;
using domis.api.DTOs.Product;
using domis.api.Models;
using domis.api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Endpoints;

//TODO: uncomment RequireAuthorization at endpoints
public static class AdminEndpoints
{
    public static void RegisterAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/admin").WithTags("Admin");

        group.MapGet("/users", async (IAdminService adminService) =>
        {
            var users = await adminService.GetUsers();
            
            return Results.Ok(users);
        })
        .WithDescription("Get all users (id, username & role).").RequireAuthorization(Roles.Admin.GetName());

        group.MapPost("/promote-to-admin", async ([FromBody] string userId, IAdminService adminService) =>
        {
            var userWithRoles = await adminService.PromoteToAdmin(userId);
            
            return userWithRoles is null
                ? Results.NotFound("User not found.")
                : Results.Ok("User promoted to admin successfully.");
        })
        .WithDescription("Promote user to admin role.").RequireAuthorization(Roles.Admin.GetName());

        group.MapGet("/user/{userId}", async (string userId, IAdminService adminService) =>
        {
            var userWithRole = await adminService.GetUserById(userId);
            
            return userWithRole is not null
                ? Results.Ok(userWithRole)
                : Results.NotFound("User not found.");
        })
        .WithDescription("Get user by ID.").RequireAuthorization(Roles.Admin.GetName());

        group.MapGet("/roles", async (IAdminService adminService) =>
        {
            var roles = await adminService.GetRoles();
            
            return Results.Ok(roles);
        })
        .WithDescription("Get all roles.").RequireAuthorization(Roles.Admin.GetName());

        group.MapPut("/roles/discount", async ([FromBody] RoleDiscountRequest request, IAdminService adminService) =>
        {
            var updatedRole = await adminService.UpdateRoleDiscount(request.RoleName, request.Discount);
            
            return updatedRole is not null
                ? Results.Ok(updatedRole)
                : Results.BadRequest($"Failed to update role: {request.RoleName}.");
        })
        .WithDescription("Update role discount.").RequireAuthorization(Roles.Admin.GetName());

        group.MapPut("/user-role/{userId}", async (string userId, [FromBody] RoleRequest request, IAdminService adminService) =>
        {
            if (!Enum.TryParse<Roles>(request.Role, true, out var role))
                return Results.BadRequest($"Role '{request.Role}' is not a valid role.");

            var success = await adminService.AddRoleToUser(userId, role);
            
            return success
                ? Results.Ok($"User promoted to {role.GetName()} successfully.")
                : Results.BadRequest($"Failed to promote user to {role.GetName()}.");

        })
        .WithDescription("Add user to a role.").RequireAuthorization(Roles.Admin.GetName());

        group.MapDelete("/user-role/{userId}", async (string userId, [FromBody] RoleRequest request, IAdminService adminService) =>
        {
            if (!Enum.TryParse<Roles>(request.Role, true, out var role))
                return Results.BadRequest($"Role '{request.Role}' is not a valid role.");

            var success = await adminService.RemoveRoleFromUser(userId, role);
            
            return success
                ? Results.Ok($"User no longer has the {role.GetName()} role.")
                : Results.BadRequest($"Failed to remove {role.GetName()} role from user.");

        })
        .WithDescription("Remove user from a role.").RequireAuthorization(Roles.Admin.GetName());

        group.MapGet("/orders", async (IAdminService adminService) =>
        {
            var response = await adminService.Orders();
            
            return Results.Ok(response);
        })
        .WithDescription("Gets all orders in the system.").RequireAuthorization(Roles.Admin.GetName());

        group.MapPut("/orders/status", async ([FromBody] UpdateOrderStatusRequest request, IOrderService orderService) =>
        {
            var response = await orderService.UpdateOrderStatus(request.OrderId, request.StatusId);

            return Results.Ok(new UpdateOrderResponse(response));
        })
        .WithDescription("Update order status(1-New, 2-In Progress, 3-Sent, 4-Completed, 5-Canceled)").RequireAuthorization(Roles.Admin.GetName());

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
        .WithDescription("Put a product on sale.").RequireAuthorization(Roles.Admin.GetName());

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
        .WithDescription(
            "Put products within category on sale. If some products are already on sale separately, they will not be put on sale and will be returned for admin to decide what to do.")
        .RequireAuthorization(Roles.Admin.GetName());

        group.MapDelete("/product/sale", async ([FromBody] List<int> request, IProductService productService) =>
            {
                var success = await productService.RemoveProductsFromSale(request);
                Results.Ok(success);
            })
        .WithDescription("Assign or update a category for a product.").RequireAuthorization(Roles.Admin.GetName());

        group.MapPost("/product/category", async ([FromBody] AssignProductToCategoryRequest request, IProductService productService) =>
        {
            var success = await productService.AssignProductToCategory(request);
            
            return success
                ? Results.Ok("Product category updated successfully.")
                : Results.BadRequest("Failed to update product category.");
        })
        .WithDescription("Assign or update a category for a product.");//.RequireAuthorization(Roles.Admin.GetName());

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
        .WithDescription("Update product sizing.").RequireAuthorization(Roles.Admin.GetName());

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
        .WithDescription("Update product sizing.").RequireAuthorization(Roles.Admin.GetName());

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
        .WithDescription("Gets history of product sale (reduced prices)").RequireAuthorization(Roles.Admin.GetName());

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
        }).WithDescription("Add new product");//.RequireAuthorization(Roles.Admin.GetName());
        
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
        }).WithDescription("Add new product");//.RequireAuthorization(Roles.Admin.GetName());
    }
}