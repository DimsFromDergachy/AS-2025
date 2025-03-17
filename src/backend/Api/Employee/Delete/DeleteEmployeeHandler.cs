using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Employee.Delete;

public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeRequest, Result<DeleteEmployeeResponse>>
{
    private readonly EmployeeService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteEmployeeHandler(
        EmployeeService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<DeleteEmployeeResponse>> Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request, cancellationToken);
        return new Result<DeleteEmployeeResponse>(new DeleteEmployeeResponse());
    }
}