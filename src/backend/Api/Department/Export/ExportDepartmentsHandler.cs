using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.Export;
using MediatR;

namespace AS_2025.Api.Department.Export;

public class ExportDepartmentsHandler : IRequestHandler<ExportDepartmentsRequest, Result<ExportDepartmentsResponse>>
{
    private const string SheetName = "Data";

    private readonly DepartmentService _departmentService;
    private readonly IXlsxDataExportService<Domain.Entities.Department> _xlsxDataExportService;

    public ExportDepartmentsHandler(
        DepartmentService departmentService, 
        IXlsxDataExportService<Domain.Entities.Department> xlsxDataExportService)
    {
        _departmentService = departmentService;
        _xlsxDataExportService = xlsxDataExportService;
    }

    public async Task<Result<ExportDepartmentsResponse>> Handle(ExportDepartmentsRequest request, CancellationToken cancellationToken)
    {
        var data = await _departmentService.ExportListAsync(request, cancellationToken);

        switch (request.ExportType)
        {
            case ExportType.Excel:
                var bytes = await _xlsxDataExportService.ExportAsync(data, SheetName, cancellationToken);
                return new ExportDepartmentsResponse(bytes, ContentTypes.Xlsx, GetFileName());

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static string GetFileName()
    {
        return $"Departments_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
    }
}