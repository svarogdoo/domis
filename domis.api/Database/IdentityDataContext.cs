using domis.api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace domis.api.Database;

public class IdentityDataContext(DbContextOptions<IdentityDataContext> options) :
        IdentityDbContext<UserEntity, Role, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserEntity>().Property(u => u.FirstName).HasMaxLength(20);

        builder.HasDefaultSchema("identity");
    }
}

