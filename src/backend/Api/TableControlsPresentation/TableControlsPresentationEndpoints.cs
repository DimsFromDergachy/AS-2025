using Ardalis.Result.AspNetCore;
using AS_2025.Api.TableControlsPresentation.List;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.TableControlsPresentation;

public static class TableControlsPresentationEndpoints
{
    public static void MapTableControlsPresentationEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/tableControlsPresentation")
            .WithTags("TableControlsPresentation");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListTableControlsPresentationsRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListTableControlsPresentationsRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet("/list/schema", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<ListTableControlsPresentationsItem>());
    }
}
