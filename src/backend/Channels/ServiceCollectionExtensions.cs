namespace AS_2025.Channels;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChannels(this IServiceCollection services)
    {
        services.AddSingleton(_ => ChannelFactory.CreateApiEventsChannel());

        return services;
    }
}