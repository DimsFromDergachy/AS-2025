using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule;

public record SimulatedAnnealingSchedulerSolution(double BestScore, IReadOnlyCollection<ProjectInWork> ProjectsInWork);

public class SimulatedAnnealingScheduler
{
    private readonly List<Team> _teams;
    private readonly List<Project> _projects;
    private readonly int _quarterDays; // длительность квартала (например, 90 дней)
    private readonly AnnealingParameters _parameters;

    private readonly Random rnd = new Random();

    public SimulatedAnnealingScheduler(List<Team> teams, List<Project> projects, int quarterDays, AnnealingParameters parameters)
    {
        _teams = teams;
        _projects = projects;
        _quarterDays = quarterDays;
        _parameters = parameters;
    }

    // Основной метод – симулированный отжиг
    public SimulatedAnnealingSchedulerSolution Solve()
    {
        // Инициализируем начальное решение: ни один проект не назначен
        ScheduleSolution currentSolution = new ScheduleSolution(_teams, _projects);
        ScheduleSolution bestSolution = currentSolution.DeepCopy();
        double bestScore = Evaluate(bestSolution);

        double temperature = _parameters.Temperature;

        while (temperature > 1)
        {
            for (int i = 0; i < _parameters.IterationsPerTemp; i++)
            {
                ScheduleSolution newSolution = GetNeighbor(currentSolution);
                double delta = Evaluate(newSolution) - Evaluate(currentSolution);

                if (delta > 0 || Math.Exp(delta / temperature) > rnd.NextDouble())
                {
                    currentSolution = newSolution;
                    double currentScore = Evaluate(currentSolution);
                    if (currentScore > bestScore)
                    {
                        bestSolution = currentSolution.DeepCopy();
                        bestScore = currentScore;
                    }
                }
            }
            temperature *= _parameters.CoolingRate;
        }

        return ConvertToResult(bestScore, bestSolution);
    }

    // Функция оценки решения
    // Оценка = сумма (q_i + c_i) для всех проектов, завершённых до конца квартала.
    private double Evaluate(ScheduleSolution sol)
    {
        double score = 0;
        foreach (var team in _teams)
        {
            int currentTime = 0;
            // Если для команды назначены проекты
            if (sol.TeamSchedules.ContainsKey(team.Id))
            {
                foreach (var proj in sol.TeamSchedules[team.Id])
                {
                    // Время на «вникание» (3 дня) + выполнение (округляем вверх)
                    int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                    currentTime += duration;
                    if (currentTime <= _quarterDays)
                    {
                        // Если проект успевают выполнить – прибавляем бонус
                        score += proj.Q + proj.C;
                    }
                    else
                    {
                        // Если проект не укладывается – дальнейшие проекты той же команды также не выполнятся
                        break;
                    }
                }
            }
        }
        return score;
    }

    // Генерация соседнего решения
    private ScheduleSolution GetNeighbor(ScheduleSolution sol)
    {
        ScheduleSolution neighbor = sol.DeepCopy();

        // Выбираем тип операции случайным образом: 0 - insert, 1 - remove, 2 - swap, 3 - move между командами
        int moveType = rnd.Next(4);

        switch (moveType)
        {
            case 0: // Insert: берем случайный проект из нераспределённых и вставляем в расписание случайной команды
                if (neighbor.Unscheduled.Count > 0)
                {
                    int projIndex = rnd.Next(neighbor.Unscheduled.Count);
                    Project proj = neighbor.Unscheduled[projIndex];
                    // Выбираем случайную команду
                    int teamIndex = rnd.Next(_teams.Count);
                    int teamId = _teams[teamIndex].Id;
                    List<Project> teamSchedule = neighbor.TeamSchedules[teamId];
                    // Случайная позиция вставки (от 0 до teamSchedule.Count включительно)
                    int pos = rnd.Next(teamSchedule.Count + 1);
                    teamSchedule.Insert(pos, proj);
                    neighbor.Unscheduled.RemoveAt(projIndex);
                }
                break;
            case 1: // Remove: удаляем случайный проект из расписания случайной команды и помещаем его в нераспределённые
                {
                    // Выбираем случайную команду, у которой есть хотя бы один проект
                    var teamsWithProjects = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
                    if (teamsWithProjects.Count > 0)
                    {
                        int teamId = teamsWithProjects[rnd.Next(teamsWithProjects.Count)];
                        List<Project> teamSchedule = neighbor.TeamSchedules[teamId];
                        int pos = rnd.Next(teamSchedule.Count);
                        Project proj = teamSchedule[pos];
                        teamSchedule.RemoveAt(pos);
                        neighbor.Unscheduled.Add(proj);
                    }
                }
                break;
            case 2: // Swap: меняем местами два проекта в расписании одной случайной команды
                {
                    var teamsWithMultiple = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count >= 2).Select(kvp => kvp.Key).ToList();
                    if (teamsWithMultiple.Count > 0)
                    {
                        int teamId = teamsWithMultiple[rnd.Next(teamsWithMultiple.Count)];
                        List<Project> teamSchedule = neighbor.TeamSchedules[teamId];
                        int pos1 = rnd.Next(teamSchedule.Count);
                        int pos2 = rnd.Next(teamSchedule.Count);
                        // Меняем, если индексы различны
                        if (pos1 != pos2)
                        {
                            var temp = teamSchedule[pos1];
                            teamSchedule[pos1] = teamSchedule[pos2];
                            teamSchedule[pos2] = temp;
                        }
                    }
                }
                break;
            case 3: // Move между командами: переносим проект из расписания одной команды в расписание другой
                {
                    var teamsWithProjects = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
                    if (teamsWithProjects.Count > 0)
                    {
                        int fromTeamId = teamsWithProjects[rnd.Next(teamsWithProjects.Count)];
                        List<Project> fromSchedule = neighbor.TeamSchedules[fromTeamId];
                        int pos = rnd.Next(fromSchedule.Count);
                        Project proj = fromSchedule[pos];
                        fromSchedule.RemoveAt(pos);
                        // Выбираем другую команду (или ту же, если других нет)
                        int toTeamId = fromTeamId;
                        if (_teams.Count > 1)
                        {
                            do
                            {
                                toTeamId = _teams[rnd.Next(_teams.Count)].Id;
                            } while (toTeamId == fromTeamId);
                        }
                        List<Project> toSchedule = neighbor.TeamSchedules[toTeamId];
                        int insertPos = rnd.Next(toSchedule.Count + 1);
                        toSchedule.Insert(insertPos, proj);
                    }
                }
                break;
        }

        return neighbor;
    }

    // Преобразование решения в итоговый список назначений с вычислением start/end для каждого проекта, укладывающегося в квартал
    private SimulatedAnnealingSchedulerSolution ConvertToResult(double bestScore, ScheduleSolution solution)
    {
        var projectsInWork = new List<ProjectInWork>();

        foreach (var team in _teams)
        {
            int currentTime = 0;
            if (solution.TeamSchedules.ContainsKey(team.Id))
            {
                foreach (var proj in solution.TeamSchedules[team.Id])
                {
                    int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                    int startTime = currentTime;
                    int endTime = currentTime + duration;
                    if (endTime <= _quarterDays)
                    {
                        projectsInWork.Add(new ProjectInWork(proj.Id, team.Id, startTime, endTime));
                        currentTime = endTime;
                    }
                    else
                    {
                        // Если проект не укладывается в сроки, дальнейшие в этой команде также не выполняются
                        break;
                    }
                }
            }
        }

        return new SimulatedAnnealingSchedulerSolution(bestScore, projectsInWork);
    }
}
