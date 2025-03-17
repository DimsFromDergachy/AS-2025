using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using MediatR;

namespace AS_2025.Api.Project.List;

public record ListProjectsRequest : IRequest<Result<ListProjectsResponse>>
{
    public ListProjectsFilter Filter { get; init; }
}
