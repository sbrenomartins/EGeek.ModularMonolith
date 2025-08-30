using EGeek.Id.Domain.Entities;
using EGeek.Id.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Id.Configuration;

public static class IdModularExtension
{
    public static void Apply(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdDbContext>()
            .AddDefaultTokenProviders();

        services.AddDbContext<IdDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("IdConnection"),
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "id"));
        });
    }
}
