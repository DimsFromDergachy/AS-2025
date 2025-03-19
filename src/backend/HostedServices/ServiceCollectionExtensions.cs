namespace AS_2025.HostedServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddTransient<IChainedHostedServiceJob, ApplicationDbInitializerJob>();
        services.AddTransient<IChainedHostedServiceJob, IdentityInitializerJob>();
        services.AddTransient<IChainedHostedServiceJob, ImportDataJob>();
        services.AddTransient<ImportDataJob>(); // for utils endpoint

        services.AddHostedService<ChainedHostedService>();
        services.AddHostedService<ApiEventProcessorBackgroundService>();

        return services;
    }
}