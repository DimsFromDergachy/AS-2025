using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using MediatR;

namespace AS_2025.Api.Department.List;

public record ListDepartmentsRequest : IRequest<Result<ListDepartmentsResponse>>
{
    public ListDepartmentsFilter Filter { get; init; }
}
