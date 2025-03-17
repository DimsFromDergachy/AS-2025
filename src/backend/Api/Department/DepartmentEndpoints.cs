using Ardalis.Result.AspNetCore;
using AS_2025.Api.Department.Delete;
using AS_2025.Api.Department.List;
using AS_2025.Api.Department.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Department;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/department")
            .WithTags("Department");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListDepartmentsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListDepartmentsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListDepartmentsItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListDepartmentsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListDepartmentsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] DeleteDepartmentRequest? request) =>
        {
            var result = await mediator.Send((request ?? new DeleteDepartmentRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });
    }
}
