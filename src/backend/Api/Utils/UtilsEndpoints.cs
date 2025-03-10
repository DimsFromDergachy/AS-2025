using AS_2025.Common;
using AS_2025.HostedServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Api.Utils;

public static class UtilsEndpoints
{
    public static void MapUtilsEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/utils")
            .WithTags("Utils");

        group.MapGet("/app-state", async ([FromServices] IContext context, CancellationToken cancellationToken) =>
        {
            // TODO 
            return context.Departments
                .Include(x => x.Teams)
                .Include(x => x.Employees)
                .ToListAsync(cancellationToken);
        });

        group.MapGet("/data-rescan", async ([FromServices] ImportDataHostedService importDataHostedService, CancellationToken cancellationToken) =>
        {
            await importDataHostedService.StartAsync(cancellationToken);
        });
    }
}