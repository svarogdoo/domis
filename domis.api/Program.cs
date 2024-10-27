using domis.api.Configuration;
using domis.api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterProductEndpoints();
app.RegisterCartEndpoints();
app.RegisterCategoryEndpoints();
app.RegisterSyncEndpoints();
app.RegisterLocationEndpoints();
app.RegisterOrderEndpoints();
app.RegisterUserEndpoints();
//app.RegisterIdentityHelperEndpoints();

app.Run();

//Populates the database with seed data

//if (app.Environment.IsDevelopment())
//{
//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//await SeedData.SeedAsync(services);