using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Api.Algos.Scheduler;

public record SchedulerResponse
{
    public IReadOnlyCollection<AS_2025.Algos.TasksSchedule.Models.TeamRequest> InputTeams { get; init; } = new List<AS_2025.Algos.TasksSchedule.Models.TeamRequest>();

    public IReadOnlyCollection<AS_2025.Algos.TasksSchedule.Models.ProjectRequest> InputProjects { get; init; } = new List<AS_2025.Algos.TasksSchedule.Models.ProjectRequest>();

    public AnnealingParameters Parameters { get; init; }
    
    public double BestScore { get; init; }
    
    public IReadOnlyCollection<ProjectInWorkResponse> ProjectsInWork { get; init; } = new List<ProjectInWorkResponse>();
}