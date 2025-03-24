using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.Export;
using MediatR;

namespace AS_2025.Api.Department.Export;

public class ExportDepartmentsHandler : IRequestHandler<ExportDepartmentsRequest, Result<ExportDepartmentsResponse>>
{
    private const string SheetName = "Data";
    private const string TemplateName = "departments";

    private readonly DepartmentService _departmentService;
    private readonly IXlsxDataExportService<Domain.Entities.Department> _xlsxDataExportService;
    private readonly ITemplateHtmlDataExportService<Domain.Entities.Department> _templateHtmlDataExportService;
    private readonly IPdfDataExportService<Domain.Entities.Department> _pdfDataExportService;

    public ExportDepartmentsHandler(
        DepartmentService departmentService, 
        IXlsxDataExportService<Domain.Entities.Department> xlsxDataExportService,
        ITemplateHtmlDataExportService<Domain.Entities.Department> templateHtmlDataExportService, 
        IPdfDataExportService<Domain.Entities.Department> pdfDataExportService)
    {
        _departmentService = departmentService;
        _xlsxDataExportService = xlsxDataExportService;
        _templateHtmlDataExportService = templateHtmlDataExportService;
        _pdfDataExportService = pdfDataExportService;
    }

    public async Task<Result<ExportDepartmentsResponse>> Handle(ExportDepartmentsRequest request, CancellationToken cancellationToken)
    {
        var data = await _departmentService.ExportListAsync(request, cancellationToken);

        switch (request.ExportType)
        {
            case ExportType.Excel:
            {
                var bytes = await _xlsxDataExportService.ExportAsync(data, SheetName, cancellationToken);
                return new ExportDepartmentsResponse(bytes, ContentTypes.Xlsx, GetFileName("xlsx"));
            }

            case ExportType.Html:
            {
                var bytes = await _templateHtmlDataExportService.ExportAsync(TemplateName, data, cancellationToken);
                return new ExportDepartmentsResponse(bytes, ContentTypes.Html, GetFileName("html"));
            }

            case ExportType.Pdf:
            {
                var bytes = await _pdfDataExportService.ExportAsync(TemplateName, data, cancellationToken);
                return new ExportDepartmentsResponse(bytes, ContentTypes.Pdf, GetFileName("pdf"));
            }

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static string GetFileName(string extension)
    {
        return $"Departments_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{extension}";
    }
}