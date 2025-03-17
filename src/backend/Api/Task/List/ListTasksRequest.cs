using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using MediatR;

namespace AS_2025.Api.Task.List;

public record ListTasksRequest : IRequest<Result<ListTasksResponse>>
{
    public ListTasksFilter Filter { get; init; }
}
