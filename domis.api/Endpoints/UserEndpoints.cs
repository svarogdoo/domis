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

            var userProfile = await userService.GetUserByIdAsync(userId);

            if (userProfile is null) return Results.NotFound();

            return Results.Ok(userProfile);
        })
        .RequireAuthorization(); 
    }
}