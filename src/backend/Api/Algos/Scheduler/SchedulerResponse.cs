using AS_2025.Algo.Common;
using AS_2025.Algo.TasksSchedule.Models;

namespace AS_2025.Api.Algos.Scheduler;

public record SchedulerResponse
{
    public IReadOnlyCollection<Algo.TasksSchedule.Models.Team> InputTeams { get; init; } = new List<Algo.TasksSchedule.Models.Team>();

    public IReadOnlyCollection<Algo.TasksSchedule.Models.Project> InputProjects { get; init; } = new List<Algo.TasksSchedule.Models.Project>();

    public AnnealingParameters Parameters { get; init; }
    
    public double BestScore { get; init; }
    
    public IReadOnlyCollection<ProjectInWork> ProjectsInWork { get; init; } = new List<ProjectInWork>();
}