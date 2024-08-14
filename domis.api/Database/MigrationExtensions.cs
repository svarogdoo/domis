using Microsoft.EntityFrameworkCore;

namespace domis.api.Database;

public static class MigrationExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}
