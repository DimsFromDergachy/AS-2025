using Ardalis.Result.AspNetCore;
using AS_2025.Api.Image.Edit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AS_2025.Api.Image;

public static class ImageEndpoints
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/image").WithTags("Image");

        group.MapPost("/edit", async (IMediator mediator, [FromForm] ImageEditRequest request) => 
        {
            var result = await mediator.Send(request);
            return result.ToMinimalApiResult();
        }).Accepts<IFormFile>("multipart/form-data").DisableAntiforgery();
    }
}