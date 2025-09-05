using EGeek.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Catalog.Configuration;

public static class CatalogModularExtension
{
    public static void Apply(IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(configurationManager.GetConnectionString("CatalogConnection"),
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "catalog"));
        });
    }
}
