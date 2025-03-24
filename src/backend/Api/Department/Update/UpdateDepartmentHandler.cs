using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Department.Update;

public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentRequest, Result<UpdateDepartmentResponse>>
{
    private readonly DepartmentService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateDepartmentHandler(
        DepartmentService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<UpdateDepartmentResponse>> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var department = await _service.UpdateAsync(request, cancellationToken);
        return Mapper.ToUpdateResponse(department);
    }
}