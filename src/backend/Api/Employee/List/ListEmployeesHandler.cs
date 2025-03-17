using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Employee.List;

public class ListEmployeesHandler : IRequestHandler<ListEmployeesRequest, Result<ListEmployeesResponse>>
{
    private readonly EmployeeService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ListEmployeesHandler(
        EmployeeService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<ListEmployeesResponse>> Handle(ListEmployeesRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(request.Filter, cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListEmployeesResponse
        {
            Items = items
        };
    }
}
