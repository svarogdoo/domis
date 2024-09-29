using domis.api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace domis.api.Database;

public class IdentityDataContext(DbContextOptions<IdentityDataContext> options) :
        IdentityDbContext<User, Role, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().Property(u => u.FirstName).HasMaxLength(20);

        builder.HasDefaultSchema("identity");
    }
}

