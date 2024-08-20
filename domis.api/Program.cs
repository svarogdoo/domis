using domis.api.BaseExtensions;
using domis.api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .CreateLogger();

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterProductEndpoints();
app.RegisterCategoryEndpoints();
app.RegisterSyncEndpoints();

app.Run();

//Populates the database with seed data

//if (app.Environment.IsDevelopment())
//{
//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//await SeedData.SeedAsync(services);