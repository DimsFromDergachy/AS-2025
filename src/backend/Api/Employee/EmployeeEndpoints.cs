using Ardalis.Result.AspNetCore;
using AS_2025.Api.Department.ReferenceList;
using AS_2025.Api.Employee.List;
using AS_2025.Api.Employee.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Employee;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/employee")
            .WithTags("Employee");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListEmployeesRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListEmployeesRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListEmployeesItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListEmployeesRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListEmployeesRequest());
            return result.ToMinimalApiResult();
        });
    }
}