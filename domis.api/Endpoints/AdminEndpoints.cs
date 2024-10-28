using domis.api.Models;
using domis.api.Services;
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
        .WithDescription("Get all users (id, username & role).");
        // .RequireAuthorization("Administrator");
        
        group.MapPost("/promote-to-admin", async ([FromBody]string userId, IAdminService adminService) =>
        {
            var userWithRoles = await adminService.PromoteToAdmin(userId);
            return userWithRoles is null 
                ? Results.NotFound("User not found.") 
                : Results.Ok("User promoted to admin successfully.");
        })
        .WithDescription("Promote user to admin role.");
        // .RequireAuthorization("Administrator");
        
        group.MapGet("/user/{userId}", async (string userId, IAdminService adminService) =>
        {
            var userWithRole = await adminService.GetUserById(userId);
            return userWithRole is not null
                ? Results.Ok(userWithRole) 
                : Results.NotFound("User not found.");
        })
        .WithDescription("Get user by ID.");
        // .RequireAuthorization("Administrator");

        group.MapGet("/roles", async (IAdminService adminService) =>
        {
            var roles = await adminService.GetRoles();
            return Results.Ok(roles);
        })
        .WithDescription("Get all roles.");
        // .RequireAuthorization("Administrator");
        
        group.MapPut("/roles/discount", async ([FromBody] DiscountRequest request, IAdminService adminService) =>
        {
            var updatedRole = await adminService.UpdateRoleDiscount(request.RoleName, request.Discount);
            return updatedRole is not null
                ? Results.Ok(updatedRole) 
                : Results.BadRequest($"Failed to update role: {request.RoleName}.");
        })
        .WithDescription("Update role discount.");
        // .RequireAuthorization("Administrator");
        
        group.MapPost("/user-role/{userId}", async (string userId, [FromBody] RoleRequest request, IAdminService adminService) =>
        {
            var success = await adminService.AddRoleToUser(userId, request.Role);
            return success 
                ? Results.Ok($"User promoted to {request.Role.RoleName()} successfully.") 
                : Results.BadRequest($"Failed to promote user to {request.Role.RoleName()}.");
        })
        .WithDescription("Add user to a role.");
        // .RequireAuthorization("Administrator");
        
        group.MapPut("/user-role/{userId}", async (string userId, [FromBody] RoleRequest request, IAdminService adminService) =>
        {
            var success = await adminService.RemoveRoleFromUser(userId, request.Role);
            return success 
                ? Results.Ok($"User no longer has the {request.Role.RoleName()} role.") 
                : Results.BadRequest($"Failed to remove {request.Role.RoleName()} role from user.");
        })
        .WithDescription("Remove user from a role.");
        // .RequireAuthorization("Administrator");

        group.MapGet("/orders", async (IAdminService adminService) =>
        {
            var response = await adminService.Orders();
            return Results.Ok(response);
        })
        .WithDescription("Gets all orders in the system.");
        // .RequireAuthorization("Administrator");

        group.MapPut("/orders/status", async ([FromBody] UpdateOrderStatusRequest request, IOrderService orderService) =>
        {
            var response = await orderService.UpdateOrderStatus(request.OrderId, request.StatusId);

            return Results.Ok(new UpdateOrderResponse(response));
        })
        .WithDescription("Update order status(1-New, 2-In Progress, 3-Sent, 4-Completed, 5-Canceled)");
        // .RequireAuthorization("Administrator");
    }
}