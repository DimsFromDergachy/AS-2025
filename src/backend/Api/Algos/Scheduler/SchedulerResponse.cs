using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Api.Algos.Scheduler;

public record SchedulerResponse
{
    public IReadOnlyCollection<AS_2025.Algos.TasksSchedule.Models.Team> InputTeams { get; init; } = new List<AS_2025.Algos.TasksSchedule.Models.Team>();

    public IReadOnlyCollection<AS_2025.Algos.TasksSchedule.Models.Project> InputProjects { get; init; } = new List<AS_2025.Algos.TasksSchedule.Models.Project>();

    public AnnealingParameters Parameters { get; init; }
    
    public double BestScore { get; init; }
    
    public IReadOnlyCollection<ProjectInWork> ProjectsInWork { get; init; } = new List<ProjectInWork>();
}