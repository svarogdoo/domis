using domis.api.Database;
using domis.api.Extensions;
using domis.api.Models;
using domis.api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace domis.api.BaseExtensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();

        //services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAuthenticationAndAuthorization();

        var connectionString = builder.Configuration.GetConnectionString("Database");

        builder.Services.AddDbContext<IdentityDataContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        builder.Services.AddScoped<IProductService, ProductService>();
    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();

            app.ApplyMigration();
        //}

        app.UseHttpsRedirection();

        app.UseCors();

        app.MapIdentityApi<User>();

        //app.UseExceptionHandler();
    }
}
