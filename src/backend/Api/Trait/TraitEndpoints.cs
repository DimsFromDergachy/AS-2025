using Ardalis.Result.AspNetCore;
using AS_2025.Api.Trait.Create;
using AS_2025.Api.Trait.List;
using AS_2025.Api.Trait.Update;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Trait;

public static class TraitEndpoints
{
    public static void MapTraitEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/trait").WithTags("Trait");

        group.MapPost("/", async (IMediator mediator, CreateTraitRequest request) =>
        {
            var result = await mediator.Send(request);
            return result.ToMinimalApiResult();
        });

        group.MapPut("{id}", async (IMediator mediator, Guid id, UpdateTraitRequest request) =>
        {
            var result = await mediator.Send(request with { Id = id });
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListTraitsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListTraitsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListTraitsItem>());
    }
}