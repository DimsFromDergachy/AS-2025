
using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Team.ReferenceList;

public class ReferenceListTeamsHandler : IRequestHandler<ReferenceListTeamsRequest, Result<ReferenceListResponse>>
{
    private readonly TeamService _service;
    private readonly ReferenceListBuilder _referenceListBuilder;

    public ReferenceListTeamsHandler(TeamService service, ReferenceListBuilder referenceListBuilder)
    {
        _service = service;
        _referenceListBuilder = referenceListBuilder;
    }

    public async Task<Result<ReferenceListResponse>> Handle(ReferenceListTeamsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken)).ToList();
        return new ReferenceListResponse
        {
            Items = _referenceListBuilder.Build(items)
        };
    }
}
