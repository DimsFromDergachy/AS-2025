using Ardalis.Result.AspNetCore;
using AS_2025.Api.Department.Create;
using AS_2025.Api.Department.Delete;
using AS_2025.Api.Department.Export;
using AS_2025.Api.Department.Get;
using AS_2025.Api.Department.List;
using AS_2025.Api.Department.ReferenceList;
using AS_2025.Api.Department.Update;
using AS_2025.Export;
using AS_2025.Schema.Form;
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

        group.MapPost("/", async (IMediator mediator, [FromBody] CreateDepartmentRequest request) =>
        {
            var result = await mediator.Send(request);
            return result.ToMinimalApiResult();
        });

        group.MapGet("/create-schema", ([FromServices] FormSchemaModelBuilder formSchemaModelBuilder) => formSchemaModelBuilder.Build<CreateDepartmentRequest>());

        group.MapPut("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] UpdateDepartmentRequest? request) =>
        {
            var result = await mediator.Send((request ?? new UpdateDepartmentRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });

        group.MapGet("/update-schema", ([FromServices] FormSchemaModelBuilder formSchemaModelBuilder) => formSchemaModelBuilder.Build<UpdateDepartmentRequest>());

        group.MapGet("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] GetDepartmentRequest? request) =>
        {
            var result = await mediator.Send((request ?? new GetDepartmentRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] DeleteDepartmentRequest? request) =>
        {
            var result = await mediator.Send((request ?? new DeleteDepartmentRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListDepartmentsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListDepartmentsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListDepartmentsItem>());

        group.MapGet("/list/export/{exportType}", async (IMediator mediator, [FromRoute] ExportType exportType, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ExportDepartmentsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ExportDepartmentsRequest() { ExportType = exportType });
            return Results.File(result.Value.Bytes, result.Value.ContentType, result.Value.FileName);
        });

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListDepartmentsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListDepartmentsRequest());
            return result.ToMinimalApiResult();
        });
    }
}
