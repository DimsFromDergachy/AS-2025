using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Department.Create;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentRequest, Result<CreateDepartmentResponse>>
{
    private readonly DepartmentService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateDepartmentHandler(
        DepartmentService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<CreateDepartmentResponse>> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var department = await _service.CreateAsync(request, cancellationToken);
        return Mapper.ToCreateResponse(department);
    }
}