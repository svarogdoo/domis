using domis.api.BaseExtensions;
using domis.api.Database;
using domis.api.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

//if (app.Environment.IsDevelopment())
//{
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await SeedData.SeedAsync(services);

app.RegisterProductEndpoints();

app.Run();