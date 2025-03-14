namespace AS_2025.ReferenceItem;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReferenceItems(this IServiceCollection services)
    {
        services.AddScoped<ReferenceListBuilder>();
        services.AddScoped<ReferenceEnumListBuilder>();

        return services;
    }
}