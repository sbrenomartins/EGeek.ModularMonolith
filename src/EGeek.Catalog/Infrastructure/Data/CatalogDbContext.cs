using EGeek.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Catalog.Infrastructure.Data;

internal class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("catalog");
    }
}
