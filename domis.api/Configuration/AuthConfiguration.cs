﻿using domis.api.Database;
using domis.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace domis.api.Extensions;

public static class AuthConfiguration
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddUserManager<CustomUserManager<User>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
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
        });

        services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorization();

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