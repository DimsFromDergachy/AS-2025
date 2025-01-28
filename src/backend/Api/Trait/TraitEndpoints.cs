using Ardalis.Result.AspNetCore;
using AS_2025.Api.Trait.Create;
using AS_2025.Api.Trait.Update;
using MediatR;

namespace AS_2025.Api.Trait;

public static class TraitEndpoints
{
    public static void MapTraitEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/trait")
            .WithTags("Trait");

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
    }
}