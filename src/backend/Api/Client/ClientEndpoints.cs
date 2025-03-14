using Ardalis.Result.AspNetCore;
using AS_2025.Api.Client.List;
using AS_2025.Api.Client.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Client;

public static class ClientEndpoints
{
    public static void MapClientEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/client")
            .WithTags("Client");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListClientsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListClientsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListClientsItem>());

        group.MapGet("/reference-list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceListClientsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceListClientsRequest());
            return result.ToMinimalApiResult();
        });
    }
}
