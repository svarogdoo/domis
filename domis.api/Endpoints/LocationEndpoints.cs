using domis.api.Services;

namespace domis.api.Endpoints;

public static class LocationEndpoints
{
    public static void RegisterLocationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/location").WithTags("Location");

        group.MapGet("/countries", async (ILocationService locationService) =>
        {
            var response = await locationService.GetAllCountries();

            return Results.Ok(response);
        }).WithDescription("Get all countries");
    }
}