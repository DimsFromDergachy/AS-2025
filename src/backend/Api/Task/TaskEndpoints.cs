﻿using Ardalis.Result.AspNetCore;
using AS_2025.Api.Task.List;
using AS_2025.Api.Task.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Task;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/task")
            .WithTags("Task").WithOrder(6);

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListTasksRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListTasksRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListTasksItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListTasksRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListTasksRequest());
            return result.ToMinimalApiResult();
        });
    }
}
