namespace AS_2025.Hubs;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder builder)
    {
        // TODO map to /hub
        builder.MapHub<ApiEventsHub>("/api-events");

        return builder;
    }
}