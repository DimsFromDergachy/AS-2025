namespace AS_2025.HostedServices;

public interface IChainedHostedServiceJob
{
    int Order { get; }

    Task RunAsync(CancellationToken cancellationToken);
}