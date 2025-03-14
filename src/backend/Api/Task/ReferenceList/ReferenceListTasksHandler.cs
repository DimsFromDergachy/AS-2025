using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Task.ReferenceList;

public class ReferenceListTasksHandler : IRequestHandler<ReferenceListTasksRequest, Result<ReferenceListResponse>>
{
    private readonly TaskService _service;
    private readonly ReferenceListBuilder _referenceListBuilder;

    public ReferenceListTasksHandler(TaskService service, ReferenceListBuilder referenceListBuilder)
    {
        _service = service;
        _referenceListBuilder = referenceListBuilder;
    }

    public async Task<Result<ReferenceListResponse>> Handle(ReferenceListTasksRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken)).ToList();
        return new ReferenceListResponse
        {
            Items = _referenceListBuilder.Build(items)
        };
    }
}
