using Ardalis.Result;
using AS_2025.ApplicationServices.Filters;
using MediatR;

namespace AS_2025.Api.Client.List;

public record ListClientsRequest : IRequest<Result<ListClientsResponse>>
{
    public ListClientsFilter Filter { get; init; }
}
