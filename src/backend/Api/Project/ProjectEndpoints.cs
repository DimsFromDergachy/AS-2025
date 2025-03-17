using Ardalis.Result.AspNetCore;
using AS_2025.Api.Project.Delete;
using AS_2025.Api.Project.List;
using AS_2025.Api.Project.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Project;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/project")
            .WithTags("Project");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListProjectsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListProjectsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListProjectsItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListProjectsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListProjectsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] DeleteProjectRequest? request) =>
        {
            var result = await mediator.Send((request ?? new DeleteProjectRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });
    }
}
