using System.ComponentModel;
using Ardalis.Result;
using AS_2025.Schema.Form;
using MediatR;

namespace AS_2025.Api.Algos.Scheduler;

[FormSchema(Title = "Scheduler configure")]
public record SchedulerRequest : IRequest<Result<SchedulerResponse>>
{
    [DefaultValue(90)]
    [FormInputSchema(Order = 1, DisplayType = FormInputDisplayType.Slider, Label = "Calculate days", NumberMin = 30, NumberMax = 180, NumberStep = 30)]
    public int QuarterDays { get; init; } = 90;

    [DefaultValue(1000d)]
    [FormInputSchema(Order = 2, DisplayType = FormInputDisplayType.Slider, Label = "Temperature", NumberMin = 500, NumberMax = 1500, NumberStep = 100)]
    public double Temperature { get; init; } = 1000d;

    [DefaultValue(0.995d)]
    [FormInputSchema(Order = 3, DisplayType = FormInputDisplayType.Slider, Label = "Cooling rate", NumberMin = 0.5d, NumberMax = 1.5d, NumberStep = 0.005d)]
    public double CoolingRate { get; init; } = 0.995d;

    [DefaultValue(100)]
    [FormInputSchema(Order = 4, DisplayType = FormInputDisplayType.Slider, Label = "Iterations per temp", NumberMin = 100, NumberMax = 1000, NumberStep = 10)]
    public int IterationsPerTemp { get; init; } = 100;
}