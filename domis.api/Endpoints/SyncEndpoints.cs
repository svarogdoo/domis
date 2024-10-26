using domis.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Endpoints;

public static class SyncEndpoints
{
    public static void RegisterSyncEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/sync").WithTags("Sync");

        group.MapPut("/nivelacija-product-update", async (ISyncService syncService) =>
        {
            var response = await syncService.NivelacijaUpdateBatch();
            return Results.Ok(response);
        }).WithDescription("update all products with values from NIVELACIJA.csv file");

        group.MapPut("/exchange-rate-update", async (ISyncService syncService) =>
        {
            var response = await syncService.UpdateExchangeRate();
            return Results.Ok(response);
        }).WithDescription("update exchange rates");

        group.MapGet("/sales-points", async (ISyncService syncService) =>
        {
            var response = await syncService.GetSalesPoints();
            return Results.Ok(response);
        }).WithDescription("gets sales points");

        group.MapGet("/print-env-variables", () =>
        {
            var sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? "Not Set";
            var sendGridFrom = Environment.GetEnvironmentVariable("SENDGRID_FROM") ?? "Not Set";
            var sendGridName = Environment.GetEnvironmentVariable("SENDGRID_NAME") ?? "Not Set";

            return Results.Ok(new
            {
                SENDGRID_API_KEY = sendGridApiKey,
                SENDGRID_FROM = sendGridFrom,
                SENDGRID_NAME = sendGridName
            });
        }).WithDescription("Prints the environment variables for SendGrid.");

        group.MapPost("/newsletter", async ([FromBody]string email, ISyncService syncService) =>
        {
            var response = await syncService.SubscribeToNewsletter(email);

            return response 
                ? Results.Ok(response) 
                : Results.BadRequest("Already subscribed.");
        });
    }
}