using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule
{
    public class GeneticScheduler
    {
        private readonly List<TeamRequest> _teams;
        private readonly List<ProjectRequest> _projects;
        private readonly int _quarterDays; // длительность квартала (например, 90 дней)
        private readonly GeneticParameters _parameters;
        private readonly Random rnd = new Random();

        public GeneticScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays, GeneticParameters parameters)
        {
            _teams = teams;
            _projects = projects;
            _quarterDays = quarterDays;
            _parameters = parameters;
        }

        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            // Создаём начальную популяцию случайных решений
            List<ScheduleSolution> population = new List<ScheduleSolution>();
            for (int i = 0; i < _parameters.PopulationSize; i++)
            {
                population.Add(new ScheduleSolution(_teams, _projects));
            }

            // Эволюционный цикл
            for (int gen = 0; gen < _parameters.Generations; gen++)
            {
                List<ScheduleSolution> newPopulation = new List<ScheduleSolution>();

                // Элитизм: сохраняем лучшее решение поколения
                var bestSol = population.OrderByDescending(sol => Evaluate(sol)).First().DeepCopy();
                newPopulation.Add(bestSol);

                // Формируем новое поколение
                while (newPopulation.Count < _parameters.PopulationSize)
                {
                    // Отбор: турнирный отбор двух родителей
                    var parent1 = TournamentSelection(population, _parameters.TournamentSize);
                    var parent2 = TournamentSelection(population, _parameters.TournamentSize);

                    // Кроссовер с вероятностью
                    ScheduleSolution child;
                    if (rnd.NextDouble() < _parameters.CrossoverRate)
                        child = Crossover(parent1, parent2);
                    else
                        child = parent1.DeepCopy();

                    // Мутация с вероятностью
                    if (rnd.NextDouble() < _parameters.MutationRate)
                        child = Mutate(child);

                    newPopulation.Add(child);
                }
                population = newPopulation;
            }

            // Выбираем лучшее решение из финальной популяции
            var bestFinal = population.OrderByDescending(sol => Evaluate(sol)).First();
            double bestScore = Evaluate(bestFinal);
            return ConvertToResult(bestScore, bestFinal);
        }

        // Турнирный отбор: выбираем случайных кандидатов и возвращаем лучший
        private ScheduleSolution TournamentSelection(List<ScheduleSolution> population, int tournamentSize)
        {
            var tournament = new List<ScheduleSolution>();
            for (int i = 0; i < tournamentSize; i++)
            {
                int idx = rnd.Next(population.Count);
                tournament.Add(population[idx]);
            }
            return tournament.OrderByDescending(sol => Evaluate(sol)).First().DeepCopy();
        }

        // Оператор кроссовера: для каждой команды случайным образом выбираем расписание из одного из родителей,
        // а для нераспределённых проектов берем объединение (без повторов)
        private ScheduleSolution Crossover(ScheduleSolution parent1, ScheduleSolution parent2)
        {
            // Новый ребёнок: пустое решение
            var child = new ScheduleSolution(_teams, new List<ProjectRequest>());

            // Для каждой команды выбираем расписание из одного из родителей
            foreach (var team in _teams)
            {
                if (rnd.NextDouble() < 0.5)
                    child.TeamSchedules[team.Id] = new List<ProjectRequest>(parent1.TeamSchedules[team.Id]);
                else
                    child.TeamSchedules[team.Id] = new List<ProjectRequest>(parent2.TeamSchedules[team.Id]);
            }

            // Собираем проекты, уже назначенные в расписаниях ребенка
            HashSet<int> scheduledIds = new HashSet<int>();
            foreach (var ts in child.TeamSchedules.Values)
            {
                foreach (var proj in ts)
                    scheduledIds.Add(proj.Id);
            }
            // Формируем список нераспределённых: объединяем нераспределённые из родителей и добавляем те, которые никуда не назначены
            var unscheduled = new List<ProjectRequest>();
            foreach (var proj in parent1.Unscheduled.Concat(parent2.Unscheduled))
            {
                if (!scheduledIds.Contains(proj.Id))
                    unscheduled.Add(proj);
            }
            foreach (var proj in _projects)
            {
                if (!scheduledIds.Contains(proj.Id) && !unscheduled.Any(p => p.Id == proj.Id))
                    unscheduled.Add(proj);
            }
            child.Unscheduled = unscheduled;
            return child;
        }

        // Оператор мутации: используем один из операторов (insert, remove, swap, move между командами)
        private ScheduleSolution Mutate(ScheduleSolution sol)
        {
            return GetNeighbor(sol);
        }

        // Генерация соседнего решения (аналог GetNeighbor из отжига)
        private ScheduleSolution GetNeighbor(ScheduleSolution sol)
        {
            ScheduleSolution neighbor = sol.DeepCopy();
            int moveType = rnd.Next(4);

            switch (moveType)
            {
                case 0: // Insert: берем случайный проект из нераспределённых и вставляем в расписание случайной команды
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
                case 1: // Remove: удаляем случайный проект из расписания случайной команды и помещаем его в нераспределённые
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
                case 2: // Swap: меняем местами два проекта в расписании одной случайной команды
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
                case 3: // Move между командами: переносим проект из расписания одной команды в расписание другой
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

        // Функция оценки решения:
        // Для каждой команды суммируем время выполнения проектов (с учётом 3 дней на вникание);
        // если проект успевают выполнить (в рамках квартала) – прибавляем q_i,
        // иначе – вычитаем c_i.
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
        private SolutionResponse<ProjectInWorkResponse> ConvertToResult(double bestScore, ScheduleSolution solution)
        {
            var projectsInWork = new List<ProjectInWorkResponse>();

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
