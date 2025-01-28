using Microsoft.Extensions.Options;

namespace AS_2025.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services, ApplicationOptions applicationOptions)
    {
        services.AddSingleton<IOptions<ApplicationOptions>>(_ => new OptionsWrapper<ApplicationOptions>(applicationOptions));
        services.AddSingleton<IOptions<DatabaseOptions>>(_ => new OptionsWrapper<DatabaseOptions>(applicationOptions.Database));
        services.AddSingleton<IOptions<HostedServicesOptions>>(_ => new OptionsWrapper<HostedServicesOptions>(applicationOptions.HostedServices));

        return services;
    }
}