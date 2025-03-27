using AS_2025.Import;
using AS_2025.Options;
using Microsoft.Extensions.Options;
using Department = AS_2025.Import.Filesystem.Model.Department;
using Employee = AS_2025.Import.Filesystem.Model.Employee;
using Task = System.Threading.Tasks.Task;
using Team = AS_2025.Import.Filesystem.Model.Team;

namespace AS_2025.HostedServices;

public class ImportDataJob : IChainedHostedServiceJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImportDataJob> _logger;

    public int Order => 3;

    public ImportDataJob(
        IServiceProvider serviceProvider, 
        ILogger<ImportDataJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var options = scope.ServiceProvider.GetRequiredService<IOptions<ApplicationOptions>>();
        if (!options.Value.HostedServices.IsEnabled(HostedServicesOptions.ImportData))
        {
            return;
        }

        switch (options.Value.Data.DataType)
        {
            case ImportDataType.Excel:
                await ImportExcel(scope.ServiceProvider, options.Value.Data.Root, cancellationToken);
                break;

            case ImportDataType.Json:
                await ImportJson(scope.ServiceProvider, options.Value.Data.Root, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task ImportExcel(IServiceProvider serviceProvider, string root, CancellationToken cancellationToken)
    {
        var map = new Dictionary<string, Func<IXlsxDataImportService>>
        {
            { "departments.xlsx", serviceProvider.GetRequiredService<XlsxDataImportService<Department>> },
            { "employees.xlsx", serviceProvider.GetRequiredService<XlsxDataImportService<Employee>> },
            { "teams.xlsx", serviceProvider.GetRequiredService<XlsxDataImportService<Team>> },
        };

        foreach (var (key, serviceSource) in map)
        {
            var dataPath = Path.Combine(root, key);

            _logger.LogInformation("Running import data from {DataPath}", dataPath);

            await serviceSource.Invoke().Import(dataPath, 0, cancellationToken);

            _logger.LogInformation("Successfully imported data from {DataPath}", dataPath);
        }

        var relationshipBuilder = serviceProvider.GetRequiredService<RelationshipBuilder>();
        await relationshipBuilder.BuildAsync(cancellationToken);
    }

    private async Task ImportJson(IServiceProvider serviceProvider, string root, CancellationToken cancellationToken)
    {
        var map = new Dictionary<string, Func<IJsonDataImportService>>
        {
            { "departments.json", serviceProvider.GetRequiredService<JsonDataImportService<Department>> },
            { "employees.json", serviceProvider.GetRequiredService<JsonDataImportService<Employee>> },
            { "teams.json", serviceProvider.GetRequiredService<JsonDataImportService<Team>> },
        };

        foreach (var (key, serviceSource) in map)
        {
            var dataPath = Path.Combine(root, key);

            _logger.LogInformation("Running import data from {DataPath}", dataPath);

            await serviceSource.Invoke().Import(dataPath, cancellationToken);

            _logger.LogInformation("Successfully imported data from {DataPath}", dataPath);
        }

        var relationshipBuilder = serviceProvider.GetRequiredService<RelationshipBuilder>();
        await relationshipBuilder.BuildAsync(cancellationToken);
    }
}