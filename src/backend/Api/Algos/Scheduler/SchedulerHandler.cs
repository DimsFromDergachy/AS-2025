using Ardalis.Result;
using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule;
using MediatR;

namespace AS_2025.Api.Algos.Scheduler;

public class SchedulerHandler : IRequestHandler<SchedulerRequest, Result<SchedulerResponse>>
{
    public Task<Result<SchedulerResponse>> Handle(SchedulerRequest request, CancellationToken cancellationToken)
    {
        var inputTeams = new List<AS_2025.Algos.TasksSchedule.Models.TeamRequest>
        {
            new(1, 2),
            new(2, 3),
            new(3, 1)
        };

        var inputProjects = new List<AS_2025.Algos.TasksSchedule.Models.ProjectRequest>
        {
            new(101, 10, 5000, 1000),
            new(102, 20, 8000, 1500),
            new(103, 5, 3000, 500),
            new(104, 15, 7000, 1200),
            new(105, 8, 4000, 800)
        };

        var parameters = new AnnealingParameters(request.Temperature, request.CoolingRate, request.IterationsPerTemp);
        var simulatedAnnealingScheduler = new SimulatedAnnealingScheduler(inputTeams,  inputProjects, request.QuarterDays, parameters);
        var (bestScore, projectsInWork) = simulatedAnnealingScheduler.Solve();

        var response = new SchedulerResponse()
        {
            InputTeams = inputTeams,
            InputProjects = inputProjects,
            Parameters = parameters,
            BestScore = bestScore,
            ProjectsInWork = projectsInWork
        };

        return System.Threading.Tasks.Task.FromResult(new Result<SchedulerResponse>(response));
    }
}