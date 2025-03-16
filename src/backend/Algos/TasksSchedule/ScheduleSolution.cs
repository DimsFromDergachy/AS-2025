using AS_2025.Algo.TasksSchedule.Models;

namespace AS_2025.Algo.TasksSchedule;

public class ScheduleSolution
{
    // Для каждой команды (ключ – team.Id) хранится список проектов, назначенных этой команде (порядок выполнения)
    public Dictionary<int, List<Project>> TeamSchedules { get; set; }
    // Список проектов, которые не назначены ни одной команде
    public List<Project> Unscheduled { get; set; }

    public ScheduleSolution(List<Team> teams, List<Project> allProjects)
    {
        TeamSchedules = new Dictionary<int, List<Project>>();
        foreach (var team in teams)
        {
            TeamSchedules[team.Id] = new List<Project>();
        }
        // Изначально все проекты не назначены
        Unscheduled = new List<Project>(allProjects);
    }

    // Глубокое копирование решения
    public ScheduleSolution DeepCopy()
    {
        var copy = new ScheduleSolution(new List<Team>(), new List<Project>());
        copy.TeamSchedules = new Dictionary<int, List<Project>>();
        foreach (var kvp in TeamSchedules)
        {
            copy.TeamSchedules[kvp.Key] = new List<Project>(kvp.Value);
        }
        copy.Unscheduled = new List<Project>(Unscheduled);
        return copy;
    }
}
;