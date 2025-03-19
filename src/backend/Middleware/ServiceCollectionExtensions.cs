namespace AS_2025.Middleware;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddSingleton<ApiEventTrackingMiddleware>();

        return services;
    }
}