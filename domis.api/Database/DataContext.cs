using domis.api.Models;
using Microsoft.EntityFrameworkCore;

namespace domis.api.Database;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("domis");

        builder.Entity<Product>()
               .ToTable("product")
               .HasKey(p => p.Id);
        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(20);
    }
}