using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using MediatR;

namespace AS_2025.Api.Team.List;

public record ListTeamsRequest : IRequest<Result<ListTeamsResponse>>
{
    public ListTeamsFilter Filter { get; init; }
}
