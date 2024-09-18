using domis.api.Database;
using domis.api.Extensions;
using domis.api.Models;
using domis.api.Repositories;
using domis.api.Services;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using System.Data;
using System.Text.Json.Serialization;

namespace domis.api.BaseExtensions;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Ensure enums are properly serialized
            }); 
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAuthenticationAndAuthorization();

        var connectionString = builder.Configuration.GetConnectionString("Database");

        builder.Services.AddDbContext<IdentityDataContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddScoped<IDbConnection>(sp =>
            new NpgsqlConnection(connectionString));

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

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();

        builder.Services.AddHttpClient<ISyncService, SyncService>();
        builder.Services.AddScoped<ISyncService, SyncService>();

        builder.Services.AddSingleton<SmtpClient>(serviceProvider =>
        {
            var client = new SmtpClient();
            // Configure the client as needed, if not already configured
            // e.g., client.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            return client;
        });

        builder.Services.AddTransient<IEmailService, EmailService>();
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