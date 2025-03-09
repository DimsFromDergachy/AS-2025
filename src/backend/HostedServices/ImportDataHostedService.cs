using AS_2025.Import;
using AS_2025.Options;
using Microsoft.Extensions.Options;

namespace AS_2025.HostedServices;

public class ImportDataHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImportDataHostedService> _logger;

    public ImportDataHostedService(IServiceProvider serviceProvider, ILogger<ImportDataHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var options = scope.ServiceProvider.GetRequiredService<IOptions<ApplicationOptions>>();
        if (!options.Value.HostedServices.IsEnabled(HostedServicesOptions.ImportData))
        {
            return;
        }

        var departmentsXlsxDataPath = Path.Combine(options.Value.Data.Root, "departments.xlsx");

        _logger.LogInformation("Running import data from {DataPath }", departmentsXlsxDataPath);

        var xlsxDataImportService = scope.ServiceProvider.GetRequiredService<XlsxDataImportService>();
        await xlsxDataImportService.Import(departmentsXlsxDataPath, 0, cancellationToken);

        _logger.LogInformation("Running import data from {DataPath}", departmentsXlsxDataPath);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}