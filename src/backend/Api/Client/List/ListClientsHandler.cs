using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Client.List;

public class ListClientsHandler : IRequestHandler<ListClientsRequest, Result<ListClientsResponse>>
{
    private readonly ClientService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ListClientsHandler(
        ClientService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ListClientsResponse>> Handle(ListClientsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(request.Filter, cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListClientsResponse
        {
            Items = items
        };
    }
}
