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
        var group = builder.MapGroup("api/utils")
            .WithTags("Utils");

        group.MapGet("/app-state", async ([FromServices] IContext context, CancellationToken cancellationToken) =>
        {
            var departmentsTask = context.Departments.AsNoTracking().ToListAsync(cancellationToken);
            var teamsTask = context.Teams.AsNoTracking().ToListAsync(cancellationToken);
            var employeesTask = context.Employees.AsNoTracking().ToListAsync(cancellationToken);

            await System.Threading.Tasks.Task.WhenAll(departmentsTask, teamsTask, employeesTask);

            return new
            {
                Departments = departmentsTask.Result,
                Teams = teamsTask.Result,
                Employees = employeesTask.Result
            };
        });

        group.MapPost("/data-rescan", async ([FromServices] ImportDataHostedService importDataHostedService, CancellationToken cancellationToken) =>
        {
            await importDataHostedService.StartAsync(cancellationToken);
        });

        group.MapGet("/reference-enums", ([FromServices] ReferenceEnumListBuilder referenceEnumListBuilder) =>
            referenceEnumListBuilder.Build(AppDomain.CurrentDomain.GetAssemblies()));

        group.MapGet("/tagged-enums", ([FromServices] TaggedEnumListBuilder taggedEnumListBuilder) =>
            taggedEnumListBuilder.Build(AppDomain.CurrentDomain.GetAssemblies()));
    }
}