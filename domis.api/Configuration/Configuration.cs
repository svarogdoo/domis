using System.Data;
using System.Text.Json.Serialization;
using domis.api.Common;
using domis.api.Database;
using domis.api.Extensions;
using domis.api.Models;
using domis.api.Models.Entities;
using domis.api.Repositories;
using domis.api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SendGrid.Extensions.DependencyInjection;
using Serilog;

namespace domis.api.Configuration;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        // Load appsettings.json and environment-specific files
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

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

        builder.Services.AddSendGrid(options =>
            options.ApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? "Sendgrid API key Not Set"
        );

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddScoped<IValidator<CreateCartItemRequest>, CreateCartItemRequestValidator>();
        //builder.Services.AddValidatorsFromAssemblyContaining<CreateCartItemRequestValidator>();

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
        builder.Services.AddScoped<ISyncRepository, SyncRepository>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IAdminService, AdminService>();

        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();

        builder.Services.AddScoped<IUserExtensionRepository, UserExtensionRepository>();

        //TODO: do we need?
        //builder.Services.AddHttpClient<ISyncService, SyncService>();
        // builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();

        builder.Services.AddSingleton<SmtpClient>(serviceProvider =>
        {
            var client = new SmtpClient();
            // Configure the client as needed, if not already configured
            // e.g., client.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            return client;
        });

        builder.Services.AddTransient<IEmailService, EmailService>();

        builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();

        //builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddTransient<ICustomEmailSender<UserEntity>, CustomEmailSender>();
        
        builder.Services.AddScoped<IPriceHelpers, PriceHelpers>();
        builder.Services.AddScoped<PriceCalculationHelper>();
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

        //app.MapIdentityApi<UserEntity>();
        app.MapCustomIdentityApi<UserEntity>();

        app.MapControllers();

        //app.UseExceptionHandler();
    }
}