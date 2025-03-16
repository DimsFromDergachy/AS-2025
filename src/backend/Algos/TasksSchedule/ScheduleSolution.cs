using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule;

public class ScheduleSolution
{
    // Для каждой команды (ключ – team.Id) хранится список проектов, назначенных этой команде (порядок выполнения)
    public Dictionary<int, List<ProjectRequest>> TeamSchedules { get; set; }
    // Список проектов, которые не назначены ни одной команде
    public List<ProjectRequest> Unscheduled { get; set; }

    public ScheduleSolution(List<TeamRequest> teams, List<ProjectRequest> allProjects)
    {
        TeamSchedules = new Dictionary<int, List<ProjectRequest>>();
        foreach (var team in teams)
        {
            TeamSchedules[team.Id] = new List<ProjectRequest>();
        }
        // Изначально все проекты не назначены
        Unscheduled = new List<ProjectRequest>(allProjects);
    }

    // Глубокое копирование решения
    public ScheduleSolution DeepCopy()
    {
        var copy = new ScheduleSolution(new List<TeamRequest>(), new List<ProjectRequest>());
        copy.TeamSchedules = new Dictionary<int, List<ProjectRequest>>();
        foreach (var kvp in TeamSchedules)
        {
            copy.TeamSchedules[kvp.Key] = new List<ProjectRequest>(kvp.Value);
        }
        copy.Unscheduled = new List<ProjectRequest>(Unscheduled);
        return copy;
    }
}
;