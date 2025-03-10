using AS_2025.Import.Filesystem.Model;
using AS_2025.Import.Handlers;

namespace AS_2025.Import;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataImportServices(this IServiceCollection services)
    {
        services.AddTransient<XlsxDataImportService<Department>>();
        services.AddTransient<IDataImportHandler<Department>, DepartmentDataImportHandler>();

        services.AddTransient<XlsxDataImportService<Employee>>();
        services.AddTransient<IDataImportHandler<Employee>, EmployeeDataImportHandler>();

        services.AddTransient<XlsxDataImportService<Team>>();
        services.AddTransient<IDataImportHandler<Team>, TeamDataImportHandler>();

        return services;
    }
}