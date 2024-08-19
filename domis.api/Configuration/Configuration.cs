using domis.api.Database;
using domis.api.Extensions;
using domis.api.Services;
using Microsoft.EntityFrameworkCore;

namespace domis.api.BaseExtensions;

public static class Configuration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddAuthenticationAndAuthorization();

        services.AddDbContext<IdentityDataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        services.AddScoped<IProductService, ProductService>();
    }
}
