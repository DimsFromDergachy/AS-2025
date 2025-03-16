using AS_2025.Common;
using AS_2025.HostedServices;
using AS_2025.ReferenceItem;
using AS_2025.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Api.Utils;

public static class UtilsEndpoints
{
    public static void MapUtilsEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/utils").WithTags("Utils");

        group.MapGet("/app-state", async ([FromServices] IContext context, CancellationToken cancellationToken) => new
        {
            Departments = await context.Departments.AsNoTracking().ToListAsync(cancellationToken),
            Teams = await context.Teams.AsNoTracking().ToListAsync(cancellationToken),
            Employees = await context.Employees.AsNoTracking().ToListAsync(cancellationToken)
        });

        group.MapPost("/data-rescan", async ([FromServices] ImportDataJob importDataJob, CancellationToken cancellationToken) =>
        {
            await importDataJob.RunAsync(cancellationToken);
        });

        group.MapGet("/reference-enums", ([FromServices] ReferenceEnumListBuilder referenceEnumListBuilder) =>
            referenceEnumListBuilder.Build(AppDomain.CurrentDomain.GetAssemblies()));

        group.MapGet("/tagged-enums", ([FromServices] TaggedEnumListBuilder taggedEnumListBuilder) =>
            taggedEnumListBuilder.Build(AppDomain.CurrentDomain.GetAssemblies()));
    }
}