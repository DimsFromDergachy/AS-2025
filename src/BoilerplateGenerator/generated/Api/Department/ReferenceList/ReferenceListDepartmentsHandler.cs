
using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Department.ReferenceList;

public class ReferenceListDepartmentsHandler : IRequestHandler<ReferenceListDepartmentsRequest, Result<ReferenceListResponse>>
{
    private readonly DepartmentService _service;
    private readonly ReferenceListBuilder _referenceListBuilder;

    public ReferenceListDepartmentsHandler(DepartmentService service, ReferenceListBuilder referenceListBuilder)
    {
        _service = service;
        _referenceListBuilder = referenceListBuilder;
    }

    public async Task<Result<ReferenceListResponse>> Handle(ReferenceListDepartmentsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken)).ToList();
        return new ReferenceListResponse
        {
            Items = _referenceListBuilder.Build(items)
        };
    }
}
