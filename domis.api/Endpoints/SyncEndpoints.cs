using domis.api.Services;

namespace domis.api.Endpoints;

public static class SyncEndpoints
{
    public static void RegisterSyncEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/sync").WithTags("Sync");

        group.MapPut("/nivelacija-update", async (ISyncService syncService) =>
        {
            var response = await syncService.NivelacijaUpdate();
            return Results.Ok(response);
        }).WithDescription("update all products with values from nivelacija file");
    }
}
