using AS_2025.Options;
using Microsoft.Extensions.Options;

namespace AS_2025.Minio;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMinioService(this IServiceCollection services, ApplicationOptions applicationOptions)
    {
        services.AddSingleton<IOptions<MinioOptions>>(_ => new OptionsWrapper<MinioOptions>(applicationOptions.Minio));
        services.AddTransient<MinioService>();

        return services;
    }
}