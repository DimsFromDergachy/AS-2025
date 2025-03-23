namespace AS_2025.Image;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageEditService(this IServiceCollection services)
    {
        services.AddTransient<ImageEditService>();

        return services;
    }
}