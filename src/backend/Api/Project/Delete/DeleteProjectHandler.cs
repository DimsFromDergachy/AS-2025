using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Project.Delete;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectRequest, Result<DeleteProjectResponse>>
{
    private readonly ProjectService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteProjectHandler(
        ProjectService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<DeleteProjectResponse>> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request, cancellationToken);
        return new Result<DeleteProjectResponse>(new DeleteProjectResponse());
    }
}