using AS_2025.Schema.Form;
using AS_2025.Schema.List;

namespace AS_2025.Schema;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchemaBuilders(this IServiceCollection services)
    {
        services.AddScoped<ListSchemaModelBuilder>();
        services.AddScoped<FormSchemaModelBuilder>();

        return services;
    }
}