using domis.api.Models;
using domis.api.Services;
using System.Security.Claims;

namespace domis.api.Endpoints;

public static class UserEndpoints
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/user").WithTags("User");

        group.MapGet("/profile", async (HttpContext http, IUserService userService) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var userProfile = await userService.UserProfile(userId);

            return userProfile is not null 
                ? Results.Ok(userProfile) 
                : Results.NotFound();
        })
        .RequireAuthorization();

        group.MapPut("/profile", async (HttpContext http, IUserService userService, ProfileUpdateRequest request) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var success = await userService.UpdateUserProfileAsync(userId, request);

            return success 
                ? Results.NoContent() 
                : Results.NotFound();
        })
        .RequireAuthorization();

        group.MapGet("/orders", async (HttpContext http, IOrderService orderService) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var userOrders = (await orderService.GetOrdersByUser(userId)).ToList();

            return userOrders.Count == 0 
                ? Results.NotFound() 
                : Results.Ok(userOrders);
        })
        .RequireAuthorization();

        group.MapPut("address", async (HttpContext http, IUserService userService, string request) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var success = await userService.UpdateUserAddressAsync(userId, request);

            return success 
                ? Results.NoContent() 
                : Results.NotFound();
        })
        .RequireAuthorization()
        .WithDescription("updates user address with provided info");

        group.MapGet("/roles", async (HttpContext http, IUserService userService) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var response = await userService.Roles(userId);

            return Results.Ok(response.ToList());
        })
        .RequireAuthorization()
        .WithDescription("Gets roles of a logged in user.");
    }
}