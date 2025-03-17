using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Employee.ReferenceList;

public class ReferenceListEmployeesHandler : IRequestHandler<ReferenceListEmployeesRequest, Result<ReferenceListResponse>>
{
    private readonly EmployeeService _service;
    private readonly ReferenceListBuilder _referenceListBuilder;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReferenceListEmployeesHandler(
        EmployeeService service, 
        ReferenceListBuilder referenceListBuilder,
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _referenceListBuilder = referenceListBuilder;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ReferenceListResponse>> Handle(ReferenceListEmployeesRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(request.Filter, cancellationToken)).ToList();
        return new ReferenceListResponse
        {
            Items = _referenceListBuilder.Build(items)
        };
    }
}
