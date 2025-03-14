using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Task.List;

public class ListTasksHandler : IRequestHandler<ListTasksRequest, Result<ListTasksResponse>>
{
    private readonly TaskService _service;

    public ListTasksHandler(TaskService service)
    {
        _service = service;
    }

    public async Task<Result<ListTasksResponse>> Handle(ListTasksRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListTasksResponse
        {
            Items = items
        };
    }
}
