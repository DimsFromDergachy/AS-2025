using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule
{
    public class PSOScheduler
    {
        private readonly List<TeamRequest> _teams;
        private readonly List<ProjectRequest> _projects;
        private readonly int _quarterDays; // длительность квартала (например, 90 дней)
        private readonly PSOParameters _parameters;
        private readonly Random rnd = new Random();

        // Частица, содержащая текущее решение, личное лучшее и соответствующие оценки
        private class Particle
        {
            public ScheduleSolution CurrentSolution { get; set; }
            public double CurrentScore { get; set; }
            public ScheduleSolution PersonalBestSolution { get; set; }
            public double PersonalBestScore { get; set; }

            public Particle(ScheduleSolution solution, double score)
            {
                CurrentSolution = solution;
                CurrentScore = score;
                PersonalBestSolution = solution.DeepCopy();
                PersonalBestScore = score;
            }
        }

        public PSOScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays, PSOParameters parameters)
        {
            _teams = teams;
            _projects = projects;
            _quarterDays = quarterDays;
            _parameters = parameters;
        }

        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            // Инициализируем популяцию частиц случайными решениями
            List<Particle> swarm = new List<Particle>();
            for (int i = 0; i < _parameters.PopulationSize; i++)
            {
                var sol = GetRandomSolution();
                double score = Evaluate(sol);
                swarm.Add(new Particle(sol, score));
            }

            // Определяем глобально лучшее решение
            Particle globalBest = swarm.OrderByDescending(p => p.CurrentScore).First();

            // Основной цикл итераций
            for (int iter = 0; iter < _parameters.Iterations; iter++)
            {
                foreach (var particle in swarm)
                {
                    // Создаем новое решение, начиная с текущего
                    ScheduleSolution newSolution = particle.CurrentSolution.DeepCopy();

                    // Случайный ход (инерция)
                    if (rnd.NextDouble() < _parameters.RandomMoveProbability)
                        newSolution = GetNeighbor(newSolution);

                    // Когнитивный компонент: приближаемся к личному лучшему решению
                    if (rnd.NextDouble() < _parameters.CognitiveCoefficient)
                        newSolution = MoveTowards(newSolution, particle.PersonalBestSolution);

                    // Социальный компонент: приближаемся к глобально лучшему решению
                    if (rnd.NextDouble() < _parameters.SocialCoefficient)
                        newSolution = MoveTowards(newSolution, globalBest.CurrentSolution);

                    double newScore = Evaluate(newSolution);
                    particle.CurrentSolution = newSolution;
                    particle.CurrentScore = newScore;

                    // Обновляем личное лучшее решение, если улучшилось
                    if (newScore > particle.PersonalBestScore)
                    {
                        particle.PersonalBestSolution = newSolution.DeepCopy();
                        particle.PersonalBestScore = newScore;
                    }

                    // Обновляем глобальное лучшее решение
                    if (newScore > globalBest.CurrentScore)
                    {
                        globalBest = particle;
                    }
                }
            }

            return ConvertToResult(globalBest.CurrentScore, globalBest.CurrentSolution);
        }

        // Генерация случайного решения (аналог метода из Монте-Карло)
        private ScheduleSolution GetRandomSolution()
        {
            var sol = new ScheduleSolution(_teams, _projects);
            List<ProjectRequest> randomProjects = _projects.OrderBy(x => rnd.NextDouble()).ToList();

            foreach (var proj in randomProjects)
            {
                if (rnd.NextDouble() < 0.7)
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

        // Оператор случайного изменения (аналог GetNeighbor из предыдущих примеров)
        private ScheduleSolution GetNeighbor(ScheduleSolution sol)
        {
            ScheduleSolution neighbor = sol.DeepCopy();
            int moveType = rnd.Next(4);

            switch (moveType)
            {
                case 0: // Insert: добавить проект из нераспределённых в расписание случайной команды
                    if (neighbor.Unscheduled.Count > 0)
                    {
                        int projIndex = rnd.Next(neighbor.Unscheduled.Count);
                        ProjectRequest proj = neighbor.Unscheduled[projIndex];
                        int teamIndex = rnd.Next(_teams.Count);
                        int teamId = _teams[teamIndex].Id;
                        List<ProjectRequest> teamSchedule = neighbor.TeamSchedules[teamId];
                        int pos = rnd.Next(teamSchedule.Count + 1);
                        teamSchedule.Insert(pos, proj);
                        neighbor.Unscheduled.RemoveAt(projIndex);
                    }
                    break;
                case 1: // Remove: удалить случайный проект из расписания случайной команды
                    {
                        var teamsWithProjects = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
                        if (teamsWithProjects.Count > 0)
                        {
                            int teamId = teamsWithProjects[rnd.Next(teamsWithProjects.Count)];
                            List<ProjectRequest> teamSchedule = neighbor.TeamSchedules[teamId];
                            int pos = rnd.Next(teamSchedule.Count);
                            ProjectRequest proj = teamSchedule[pos];
                            teamSchedule.RemoveAt(pos);
                            neighbor.Unscheduled.Add(proj);
                        }
                    }
                    break;
                case 2: // Swap: поменять местами два проекта в расписании одной команды
                    {
                        var teamsWithMultiple = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count >= 2).Select(kvp => kvp.Key).ToList();
                        if (teamsWithMultiple.Count > 0)
                        {
                            int teamId = teamsWithMultiple[rnd.Next(teamsWithMultiple.Count)];
                            List<ProjectRequest> teamSchedule = neighbor.TeamSchedules[teamId];
                            int pos1 = rnd.Next(teamSchedule.Count);
                            int pos2 = rnd.Next(teamSchedule.Count);
                            if (pos1 != pos2)
                            {
                                var temp = teamSchedule[pos1];
                                teamSchedule[pos1] = teamSchedule[pos2];
                                teamSchedule[pos2] = temp;
                            }
                        }
                    }
                    break;
                case 3: // Move: переместить проект из расписания одной команды в расписание другой
                    {
                        var teamsWithProjects = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
                        if (teamsWithProjects.Count > 0)
                        {
                            int fromTeamId = teamsWithProjects[rnd.Next(teamsWithProjects.Count)];
                            List<ProjectRequest> fromSchedule = neighbor.TeamSchedules[fromTeamId];
                            int pos = rnd.Next(fromSchedule.Count);
                            ProjectRequest proj = fromSchedule[pos];
                            fromSchedule.RemoveAt(pos);
                            int toTeamId = fromTeamId;
                            if (_teams.Count > 1)
                            {
                                do
                                {
                                    toTeamId = _teams[rnd.Next(_teams.Count)].Id;
                                } while (toTeamId == fromTeamId);
                            }
                            List<ProjectRequest> toSchedule = neighbor.TeamSchedules[toTeamId];
                            int insertPos = rnd.Next(toSchedule.Count + 1);
                            toSchedule.Insert(insertPos, proj);
                        }
                    }
                    break;
            }

            return neighbor;
        }

        // Функция, направляющая текущее решение к целевому (target).
        // В данной реализации выбирается случайная команда, и для каждого проекта из расписания target этой команды,
        // которого нет в текущем решении, выполняется попытка «переназначить» его в текущем решении.
        private ScheduleSolution MoveTowards(ScheduleSolution current, ScheduleSolution target)
        {
            ScheduleSolution result = current.DeepCopy();

            // Выбираем случайную команду
            int teamId = _teams[rnd.Next(_teams.Count)].Id;
            var targetSchedule = target.TeamSchedules[teamId];
            var currentSchedule = result.TeamSchedules[teamId];

            // Для каждого проекта в расписании целевого решения, которого нет в текущем расписании,
            // с некоторой вероятностью переносим его в расписание данной команды.
            foreach (var proj in targetSchedule)
            {
                if (!currentSchedule.Any(p => p.Id == proj.Id))
                {
                    if (rnd.NextDouble() < 0.5)
                    {
                        // Если проект встречается в другом расписании, удаляем его оттуда
                        foreach (var kvp in result.TeamSchedules)
                        {
                            kvp.Value.RemoveAll(p => p.Id == proj.Id);
                        }
                        // Также удаляем из нераспределённых
                        result.Unscheduled.RemoveAll(p => p.Id == proj.Id);
                        // Вставляем проект в случайную позицию в расписании выбранной команды
                        int pos = currentSchedule.Count > 0 ? rnd.Next(currentSchedule.Count + 1) : 0;
                        currentSchedule.Insert(pos, proj);
                    }
                }
            }
            return result;
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

        // Преобразование решения в итоговый список назначений с вычислением start/end для каждого проекта
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
                        else break;
                    }
                }
            }

            return new SolutionResponse<ProjectInWorkResponse>(projectsInWork, bestScore);
        }
    }
}
