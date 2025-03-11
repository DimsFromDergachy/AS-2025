using Ardalis.Result.AspNetCore;
using AS_2025.Api.Department.List;
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

        group.MapGet("/list/schema", ([FromServices] IListSchemaModelBuilder<ListDepartmentsItem> schemaModelBuilder) => schemaModelBuilder.Build());
    }
}