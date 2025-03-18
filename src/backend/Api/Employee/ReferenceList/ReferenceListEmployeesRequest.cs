using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Employee.ReferenceList;

public record ReferenceListEmployeesRequest : BaseQueryRequest<ReferenceListEmployeesRequest>, IRequest<Result<ReferenceListResponse>>
{
    public ListEmployeesFilter? Filter { get; init; }
}
