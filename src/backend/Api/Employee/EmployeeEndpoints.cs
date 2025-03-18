using Ardalis.Result.AspNetCore;
using AS_2025.Api.Employee.Delete;
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

        group.MapGet("/list", async (IMediator mediator, [FromQuery] ListEmployeesRequest? query) =>
        {
            var result = await mediator.Send(query ?? new ListEmployeesRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListEmployeesItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromQuery] ReferenceListEmployeesRequest? query) =>
        {
            var result = await mediator.Send(query ?? new ReferenceListEmployeesRequest());
            return result.ToMinimalApiResult();
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] DeleteEmployeeRequest? request) =>
        {
            var result = await mediator.Send((request ?? new DeleteEmployeeRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });
    }
}
