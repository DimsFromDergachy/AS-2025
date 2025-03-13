
using Ardalis.Result;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Team.ReferenceList;

public record ReferenceListTeamsRequest : IRequest<Result<ReferenceListResponse>>;
