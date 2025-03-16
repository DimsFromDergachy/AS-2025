using System.ComponentModel;
using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Algos.Scheduler;

public record SchedulerRequest : IRequest<Result<SchedulerResponse>>
{
    [DefaultValue(90)]
    public int QuarterDays { get; init; } = 90;

    [DefaultValue(1000d)]
    public double Temperature { get; init; } = 1000d;

    [DefaultValue(0.995d)]
    public double CoolingRate { get; init; } = 0.995d;

    [DefaultValue(100)]
    public int IterationsPerTemp { get; init; } = 100;
}