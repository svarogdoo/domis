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

            var userProfile = await userService.GetUserProfile(userId);

            if (userProfile is null) return Results.NotFound();

            return Results.Ok(userProfile);
        })
        .RequireAuthorization();

        group.MapPut("/profile", async (HttpContext http, IUserService userService, ProfileUpdateRequest request) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var success = await userService.UpdateUserProfileAsync(userId, request);

            if (!success) return Results.NotFound(); // or another appropriate result

            return Results.NoContent(); // 204 No Content if update was successful
        })
        .RequireAuthorization();

        group.MapGet("/orders", async (HttpContext http, IOrderService orderService) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var userOrders = await orderService.GetOrdersByUser(userId);

            if (userOrders is null || !userOrders.Any()) return Results.NotFound();

            return Results.Ok(userOrders);
        })
        .RequireAuthorization();

        group.MapPut("address", async (HttpContext http, IUserService userService, string request) =>
        {
            var userId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Results.Unauthorized();

            var success = await userService.UpdateUserAddressAsync(userId, request);

            if (!success) return Results.NotFound();

            return Results.NoContent();
        }).RequireAuthorization()
        .WithDescription("updates user address with provided info");
    }
}