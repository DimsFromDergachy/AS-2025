
using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Team.List;

public class ListTeamsHandler : IRequestHandler<ListTeamsRequest, Result<ListTeamsResponse>>
{
    private readonly TeamService _service;

    public ListTeamsHandler(TeamService service)
    {
        _service = service;
    }

    public async Task<Result<ListTeamsResponse>> Handle(ListTeamsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListTeamsResponse
        {
            Items = items
        };
    }
}
