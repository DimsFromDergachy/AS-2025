using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Project.List;

public class ListProjectsHandler : IRequestHandler<ListProjectsRequest, Result<ListProjectsResponse>>
{
    private readonly ProjectService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ListProjectsHandler(
        ProjectService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ListProjectsResponse>> Handle(ListProjectsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(request.Filter, cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListProjectsResponse
        {
            Items = items
        };
    }
}
