using Ardalis.Result.AspNetCore;
using AS_2025.Api.Algos.Backpack;
using AS_2025.Api.Algos.Scheduler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Algos;

public static class AlgosEndpoints
{
    public static void MapAlgosEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/algos").WithTags("Algos");

        group.MapPost("/backpack", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] BackpackRequest? request) =>
        {
            var result = await mediator.Send(request ?? new BackpackRequest { Capacity = 15, Temperature = 1000d, CoolingRate = 0.995d, IterationsPerTemp = 100 });
            return result.ToMinimalApiResult();
        });

        group.MapPost("/scheduler", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] SchedulerRequest? request) =>
        {
            var result = await mediator.Send(request ?? new SchedulerRequest { QuarterDays = 90, Temperature = 1000d, CoolingRate = 0.995d, IterationsPerTemp = 100 });
            return result.ToMinimalApiResult();
        });
    }
}
