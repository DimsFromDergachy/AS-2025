using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Task.ReferenceList;

public record ReferenceListTasksRequest : IRequest<Result<ReferenceListResponse>>
{
    public ListTasksFilter Filter { get; init; }
}
