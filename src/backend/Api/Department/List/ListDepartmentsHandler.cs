﻿using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Department.List;

public class ListDepartmentsHandler : IRequestHandler<ListDepartmentsRequest, Result<ListDepartmentsResponse>>
{
    private readonly DepartmentService _service;

    public ListDepartmentsHandler(DepartmentService service)
    {
        _service = service;
    }

    public async Task<Result<ListDepartmentsResponse>> Handle(ListDepartmentsRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new ListDepartmentsResponse
        {
            Items = items
        };
    }
}