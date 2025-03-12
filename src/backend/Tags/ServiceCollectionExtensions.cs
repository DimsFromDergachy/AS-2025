namespace AS_2025.Tags;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTags(this IServiceCollection services)
    {
        services.AddScoped<TaggedEnumListBuilder>();

        return services;
    }
}