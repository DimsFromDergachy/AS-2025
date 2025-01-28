namespace AS_2025.Repository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ITraitRepository, TraitRepository>();

        return services;
    }
}