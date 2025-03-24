using AS_2025.Domain.Entities;
using AS_2025.Export.Converter;
using AS_2025.Options;
using Microsoft.Extensions.Options;

namespace AS_2025.Export;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExport(this IServiceCollection services, ApplicationOptions applicationOptions)
    {
        services.AddSingleton<IOptions<TemplatesOptions>>(_ => new OptionsWrapper<TemplatesOptions>(applicationOptions.Templates));

        services.AddTransient<IXlsxDataExportService<Department>, XlsxDataExportService<Department, Model.Department>>();
        services.AddTransient<ITemplateHtmlDataExportService<Department>, TemplateHtmlDataExportService<Department, Model.Department>>();
        services.AddTransient<IPdfDataExportService<Department>, PdfDataExportService<Department>>();

        services.AddTransient<IExportModelConverter<Department, Model.Department>, DepartmentExportModelConverter>();

        return services;
    }
}