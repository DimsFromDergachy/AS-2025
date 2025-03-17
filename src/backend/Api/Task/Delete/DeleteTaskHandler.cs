using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Task.Delete;

public class DeleteTaskHandler : IRequestHandler<DeleteTaskRequest, Result<DeleteTaskResponse>>
{
    private readonly TaskService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteTaskHandler(
        TaskService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<DeleteTaskResponse>> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request, cancellationToken);
        return new Result<DeleteTaskResponse>(new DeleteTaskResponse());
    }
}