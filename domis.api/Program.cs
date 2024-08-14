using domis.api.BaseExtensions;
using domis.api.Database;
using domis.api.Extensions;
using domis.api.Models;
using domis.api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

//app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigration();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await SeedData.SeedAsync(services);
}

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();