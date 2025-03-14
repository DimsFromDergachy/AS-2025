using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Client.List;

public class ListClientsHandler : IRequestHandler<ListClientsRequest, Result<ListClientsResponse>>
{
    private readonly ClientService _service;

    public ListClientsHandler(ClientService service)
    {
        _service = service;
    }

    public async Task<Result<ListClientsResponse>> Handle(ListClientsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListClientsResponse
        {
            Items = items
        };
    }
}
