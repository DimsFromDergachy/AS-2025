using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using MediatR;

namespace AS_2025.Api.Employee.List;

public record ListEmployeesRequest : BaseQueryRequest<ListEmployeesRequest>, IRequest<Result<ListEmployeesResponse>>
{
    public ListEmployeesFilter? Filter { get; init; }
}
