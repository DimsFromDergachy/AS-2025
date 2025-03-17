using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Team.List;

public class ListTeamsHandler : IRequestHandler<ListTeamsRequest, Result<ListTeamsResponse>>
{
    private readonly TeamService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ListTeamsHandler(
        TeamService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ListTeamsResponse>> Handle(ListTeamsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(request.Filter, cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListTeamsResponse
        {
            Items = items
        };
    }
}
