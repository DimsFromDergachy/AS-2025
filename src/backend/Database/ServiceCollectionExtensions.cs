using AS_2025.Common;
using AS_2025.Options;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, ApplicationOptions applicationOptions)
    {
        services.AddScoped<IContext, ApplicationDbContext>();

        services.AddDbContextPool<ApplicationDbContext>(o =>
        {
            o.UseNpgsql(applicationOptions.Database.ConnectionString);
            o.UseExceptionProcessor();
        });

        return services;
    }

    public static IServiceCollection AddDatabaseHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<ApplicationDbInitializerHostedService>();

        return services;
    }
}