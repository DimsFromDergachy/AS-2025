using AS_2025.Minio;
using Microsoft.AspNetCore.Mvc;

namespace AS_2025.Api.Minio;

public static class MinioEndpoints
{
    public static void MapMinioEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/minio").WithTags("Minio");

        group.MapGet("/get/{id}", async ([FromRoute] string id, [FromServices] MinioService minioService, CancellationToken cancellationToken) =>
        {
            var getFileInfo = await minioService.GetAsync(id, cancellationToken);
            return Results.File(getFileInfo.Content.ToArray(), getFileInfo.ContentType, getFileInfo.FileName);
        });
    }
}