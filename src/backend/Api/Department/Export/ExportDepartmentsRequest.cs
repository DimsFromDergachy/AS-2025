using Ardalis.Result;
using AS_2025.Export;
using MediatR;

namespace AS_2025.Api.Department.Export;

public record ExportDepartmentsRequest : IRequest<Result<ExportDepartmentsResponse>>
{
    public ExportType ExportType { get; init; }
}