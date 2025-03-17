using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Department.Delete;

public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentRequest, Result<DeleteDepartmentResponse>>
{
    private readonly DepartmentService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteDepartmentHandler(
        DepartmentService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<DeleteDepartmentResponse>> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request, cancellationToken);
        return new Result<DeleteDepartmentResponse>(new DeleteDepartmentResponse());
    }
}