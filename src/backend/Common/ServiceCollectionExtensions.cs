using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace AS_2025.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCompressionSetup(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Fastest
        );

        services.Configure<GzipCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Fastest
        );

        return services;
    }

    public static IServiceCollection AddMediatRSetup(this IServiceCollection services)
    {
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker));
            config.AddOpenBehavior(typeof(ValidationResultPipelineBehavior<,>));
        });

        return services;
    }
}