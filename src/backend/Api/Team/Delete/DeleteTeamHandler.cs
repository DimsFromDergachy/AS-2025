using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Team.Delete;

public class DeleteTeamHandler : IRequestHandler<DeleteTeamRequest, Result<DeleteTeamResponse>>
{
    private readonly TeamService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteTeamHandler(
        TeamService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<DeleteTeamResponse>> Handle(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request, cancellationToken);
        return new Result<DeleteTeamResponse>(new DeleteTeamResponse());
    }
}