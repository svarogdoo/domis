using Microsoft.EntityFrameworkCore;

namespace domis.api.Database;

public static class MigrationExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using IdentityDataContext context = scope.ServiceProvider.GetRequiredService<IdentityDataContext>();
        context.Database.Migrate();
    }
}
