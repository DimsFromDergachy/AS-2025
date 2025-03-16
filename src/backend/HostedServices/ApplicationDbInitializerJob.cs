using AS_2025.Database;
using AS_2025.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AS_2025.HostedServices;

public class ApplicationDbInitializerJob : IChainedHostedServiceJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationDbInitializerJob> _logger;

    public int Order => 0;

    public ApplicationDbInitializerJob(
        IServiceProvider serviceProvider,
        ILogger<ApplicationDbInitializerJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var options = scope.ServiceProvider.GetRequiredService<IOptions<ApplicationOptions>>();
        if (!options.Value.HostedServices.IsEnabled(HostedServicesOptions.ApplicationDbInitializer))
        {
            return;
        }

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var strategy = context.Database.CreateExecutionStrategy();

        _logger.LogInformation("Running migrations for {Context}", nameof(ApplicationDbContext));

        await strategy.ExecuteAsync(async () => await context.Database.MigrateAsync(cancellationToken: cancellationToken));
        await context.Database.MigrateAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("Migrations applied successfully");
    }
}