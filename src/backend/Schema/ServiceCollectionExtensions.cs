using AS_2025.Api.Trait.List;
using AS_2025.Schema.List;

namespace AS_2025.Schema;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchemaBuilders(this IServiceCollection services)
    {
        services.AddScoped<IListSchemaModelBuilder<ListTraitsItem>, ListTraitsSchemaModelBuilder>();

        return services;
    }
}