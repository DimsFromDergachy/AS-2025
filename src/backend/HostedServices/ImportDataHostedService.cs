using AS_2025.Import;
using AS_2025.Import.Filesystem.Model;
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

        _logger.LogInformation("Running import data from {DataPath}", departmentsXlsxDataPath);

        var xlsxDepartmentDataImportService = scope.ServiceProvider.GetRequiredService<XlsxDataImportService<Department>>();
        await xlsxDepartmentDataImportService.Import(departmentsXlsxDataPath, 0, cancellationToken);

        _logger.LogInformation("Successfully imported data from {DataPath}", departmentsXlsxDataPath);

        var employeesXlsxDataPath = Path.Combine(options.Value.Data.Root, "employees.xlsx");

        _logger.LogInformation("Running import data from {DataPath}", departmentsXlsxDataPath);

        var xlsxEmployeeDataImportService = scope.ServiceProvider.GetRequiredService<XlsxDataImportService<Employee>>();
        await xlsxEmployeeDataImportService.Import(employeesXlsxDataPath, 0, cancellationToken);

        _logger.LogInformation("Successfully imported data from {DataPath}", employeesXlsxDataPath);

        var teamsXlsxDataPath = Path.Combine(options.Value.Data.Root, "teams.xlsx");

        _logger.LogInformation("Running import data from {DataPath}", departmentsXlsxDataPath);

        var xlsxTeamDataImportService = scope.ServiceProvider.GetRequiredService<XlsxDataImportService<Team>>();
        await xlsxTeamDataImportService.Import(teamsXlsxDataPath, 0, cancellationToken);

        _logger.LogInformation("Successfully imported data from {DataPath}", teamsXlsxDataPath);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}