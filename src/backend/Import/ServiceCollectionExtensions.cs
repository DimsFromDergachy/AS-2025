using AS_2025.Import.Filesystem.Model;
using AS_2025.Import.Handlers;

namespace AS_2025.Import;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataImportServices(this IServiceCollection services)
    {
        services.AddTransient<XlsxDataImportService>();

        services.AddTransient<IDataImportHandler<Department>, DepartmentDataImportHandler>();

        return services;
    }
}