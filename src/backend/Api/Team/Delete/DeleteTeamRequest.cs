using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Team.Delete;

public record DeleteTeamRequest : IRequest<Result<DeleteTeamResponse>>
{
    public Guid Id { get; init; }
}