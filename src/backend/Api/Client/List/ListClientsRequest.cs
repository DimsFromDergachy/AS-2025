using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Client.List;

public record ListClientsRequest : IRequest<Result<ListClientsResponse>>;
