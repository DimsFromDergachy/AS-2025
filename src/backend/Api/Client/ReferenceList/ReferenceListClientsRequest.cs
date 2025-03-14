using Ardalis.Result;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Client.ReferenceList;

public record ReferenceListClientsRequest : IRequest<Result<ReferenceListResponse>>;
