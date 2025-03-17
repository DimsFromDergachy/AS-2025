using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Project.ReferenceList;

public record ReferenceListProjectsRequest : IRequest<Result<ReferenceListResponse>>
{
    public ListProjectsFilter Filter { get; init; }
}
