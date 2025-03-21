﻿using Ardalis.Result.AspNetCore;
using AS_2025.Api.Team.Delete;
using AS_2025.Api.Team.List;
using AS_2025.Api.Team.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Team;

public static class TeamEndpoints
{
    public static void MapTeamEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/team")
            .WithTags("Team");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListTeamsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListTeamsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListTeamsItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListTeamsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListTeamsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, [FromRoute] Guid id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] DeleteTeamRequest? request) =>
        {
            var result = await mediator.Send((request ?? new DeleteTeamRequest()) with { Id = id });
            return result.ToMinimalApiResult();
        });
    }
}
