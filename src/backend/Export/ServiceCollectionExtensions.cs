using AS_2025.Domain.Entities;
using AS_2025.Export.Converter;

namespace AS_2025.Export;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExport(this IServiceCollection services)
    {
        services.AddTransient<IXlsxDataExportService<Department>, XlsxDataExportService<Department, Model.Department>>();
        services.AddTransient<IExportModelConverter<Department, Model.Department>, DepartmentExportModelConverter>();

        return services;
    }
}