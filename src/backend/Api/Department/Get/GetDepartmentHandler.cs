using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Department.Get;

public class GetDepartmentHandler : IRequestHandler<GetDepartmentRequest, Result<GetDepartmentResponse>>
{
    private readonly DepartmentService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetDepartmentHandler(
        DepartmentService service,
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<GetDepartmentResponse>> Handle(GetDepartmentRequest request, CancellationToken cancellationToken)
    {
        var department = await _service.GetAsync(request, cancellationToken);
        return Mapper.ToGetResponse(department);
    }
}