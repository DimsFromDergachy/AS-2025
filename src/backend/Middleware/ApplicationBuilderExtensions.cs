namespace AS_2025.Middleware;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ApiEventTrackingMiddleware>();

        return builder;
    }
}