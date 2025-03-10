namespace AS_2025.HostedServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<ApplicationDbInitializerHostedService>();
        services.AddHostedService<ImportDataHostedService>();
        services.AddSingleton<ImportDataHostedService>(); // to call from utils endpoint
            
        return services;
    }
}