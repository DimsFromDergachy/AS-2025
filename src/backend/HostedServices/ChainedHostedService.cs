namespace AS_2025.HostedServices;

public class ChainedHostedService : IHostedService
{
    private readonly IEnumerable<IChainedHostedServiceJob> _jobs;

    public ChainedHostedService(IEnumerable<IChainedHostedServiceJob> jobs)
    {
        _jobs = jobs.OrderBy(x => x.Order);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var job in _jobs)
        {
            await job.RunAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}