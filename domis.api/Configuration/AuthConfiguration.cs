using domis.api.Database;
using domis.api.Extensions;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace domis.api.Configuration;

public static class AuthConfiguration
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        services.AddIdentityCore<UserEntity>()
                .AddRoles<Role>()
                .AddUserManager<CustomUserManager<UserEntity>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddApiEndpoints();

        //TO-DO: Restore password requirements in production
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 1;

            //options.SignIn.RequireConfirmedEmail = true;
        });

        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme, options => {
            options.BearerTokenExpiration = TimeSpan.FromHours(12);
        });
     
        services.AddAuthorizationBuilder()
             .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
             .AddPolicy("VP1", policy => policy.RequireRole("VP1"))
             .AddPolicy("VP2", policy => policy.RequireRole("VP2"))
             .AddPolicy("VP3", policy => policy.RequireRole("VP3"))
             .AddPolicy("VP4", policy => policy.RequireRole("VP4"))
             .AddPolicy("User", policy => policy.RequireRole("User"));

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}