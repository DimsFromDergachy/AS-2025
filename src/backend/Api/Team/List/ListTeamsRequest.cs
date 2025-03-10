using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Team.List;

public record ListTeamsRequest : IRequest<Result<ListTeamsResponse>>;