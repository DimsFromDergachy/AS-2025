using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule
{
    public class MonteCarloScheduler
    {
        private readonly List<TeamRequest> _teams;
        private readonly List<ProjectRequest> _projects;
        private readonly int _quarterDays; // длительность квартала (например, 90 дней)
        private readonly int _iterations;  // число итераций (число случайных решений)
        private readonly Random rnd = new Random();

        public MonteCarloScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays, int iterations)
        {
            _teams = teams;
            _projects = projects;
            _quarterDays = quarterDays;
            _iterations = iterations;
        }

        // Основной метод, генерирующий случайные решения и выбирающий лучшее
        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            double bestScore = double.NegativeInfinity;
            ScheduleSolution bestSolution = new ScheduleSolution(_teams, _projects);

            for (int i = 0; i < _iterations; i++)
            {
                ScheduleSolution sol = GetRandomSolution();
                double score = Evaluate(sol);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestSolution = sol.DeepCopy();
                }
            }

            return ConvertToResult(bestScore, bestSolution);
        }

        // Генерация случайного решения:
        // для каждого проекта в случайном порядке с вероятностью 70% назначаем его в случайную команду.
        private ScheduleSolution GetRandomSolution()
        {
            var sol = new ScheduleSolution(_teams, _projects);
            List<ProjectRequest> randomProjects = _projects.OrderBy(x => rnd.NextDouble()).ToList();

            foreach (var proj in randomProjects)
            {
                if (rnd.NextDouble() < 0.7) // вероятность назначения проекта
                {
                    int teamIndex = rnd.Next(_teams.Count);
                    int teamId = _teams[teamIndex].Id;
                    List<ProjectRequest> teamSchedule = sol.TeamSchedules[teamId];
                    int pos = teamSchedule.Count > 0 ? rnd.Next(teamSchedule.Count + 1) : 0;
                    teamSchedule.Insert(pos, proj);
                    sol.Unscheduled.RemoveAll(p => p.Id == proj.Id);
                }
            }

            return sol;
        }

        // Функция оценки решения:
        // Для каждой команды суммируем время выполнения проектов (учитывая 3 дня на "вникание");
        // если проект укладывается в сроки квартала – прибавляем q_i, иначе – вычитаем c_i.
        private double Evaluate(ScheduleSolution sol)
        {
            HashSet<int> completedProjects = new HashSet<int>();

            foreach (var team in _teams)
            {
                int currentTime = 0;
                if (sol.TeamSchedules.ContainsKey(team.Id))
                {
                    foreach (var proj in sol.TeamSchedules[team.Id])
                    {
                        int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                        currentTime += duration;
                        if (currentTime <= _quarterDays)
                            completedProjects.Add(proj.Id);
                        else
                            break;
                    }
                }
            }

            double score = 0;
            foreach (var proj in _projects)
            {
                if (completedProjects.Contains(proj.Id))
                    score += proj.Q;
                else
                    score -= proj.C;
            }

            return score;
        }

        // Преобразование решения в итоговый список назначений с вычислением start/end для каждого проекта, укладывающегося в квартал.
        private SolutionResponse<ProjectInWorkResponse> ConvertToResult(double bestScore, ScheduleSolution sol)
        {
            var projectsInWork = new List<ProjectInWorkResponse>();

            foreach (var team in _teams)
            {
                int currentTime = 0;
                if (sol.TeamSchedules.ContainsKey(team.Id))
                {
                    foreach (var proj in sol.TeamSchedules[team.Id])
                    {
                        int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                        int startTime = currentTime;
                        int endTime = currentTime + duration;
                        if (endTime <= _quarterDays)
                        {
                            projectsInWork.Add(new ProjectInWorkResponse(proj.Id, team.Id, startTime, endTime));
                            currentTime = endTime;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return new SolutionResponse<ProjectInWorkResponse>(projectsInWork, bestScore);
        }
    }
}
