using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule
{
    public class DynamicProgrammingScheduler
    {
        private readonly List<TeamRequest> _teams;
        private readonly List<ProjectRequest> _projects;
        private readonly int _quarterDays; // длительность квартала, например 90 дней

        // Для восстановления решения будем сохранять для каждого состояния принятое решение:
        // decision = -1, если проект i не назначается,
        // или decision = j, если проект i назначается команде с индексом j (среди _teams)
        private readonly Dictionary<(int index, string capKey), (double profit, int decision)> memo = new();

        public DynamicProgrammingScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays)
        {
            _teams = teams;
            _projects = projects;
            _quarterDays = quarterDays;
        }

        // Основной метод – динамическое программирование.
        // Возвращает оптимальное значение суммы profit (сумма (Q+C) для назначенных проектов)
        // из состояния (index, capacities), где capacities – массив оставшихся дней для каждой команды.
        private double DP(int index, int[] capacities)
        {
            if (index == _projects.Count)
                return 0;

            string capKey = string.Join(",", capacities);
            var stateKey = (index, capKey);
            if (memo.ContainsKey(stateKey))
                return memo[stateKey].profit;

            // Вариант "не назначать" проект index.
            double best = DP(index + 1, capacities);
            int bestDecision = -1;

            // Попробуем назначить проект каждой из команд.
            ProjectRequest proj = _projects[index];
            // profit = (Q + C) для проекта
            double projProfit = proj.Q + proj.C;

            for (int j = 0; j < _teams.Count; j++)
            {
                // Рассчитываем длительность для проекта proj, если его выполняет команда j.
                int duration = 3 + (int)Math.Ceiling((double)proj.T / _teams[j].Efficiency);
                if (capacities[j] >= duration)
                {
                    int[] newCap = (int[])capacities.Clone();
                    newCap[j] -= duration;
                    double option = projProfit + DP(index + 1, newCap);
                    if (option > best)
                    {
                        best = option;
                        bestDecision = j;
                    }
                }
            }

            memo[stateKey] = (best, bestDecision);
            return best;
        }

        // Вспомогательный метод для восстановления решений (назначений) для проектов.
        // Возвращает словарь: ключ – Id проекта, значение – Id команды, к которой проект назначен.
        // Если проект не назначен, его записи нет.
        private Dictionary<int, int> ReconstructAssignments()
        {
            Dictionary<int, int> assignments = new();
            int[] capacities = Enumerable.Repeat(_quarterDays, _teams.Count).ToArray();
            for (int i = 0; i < _projects.Count; i++)
            {
                string capKey = string.Join(",", capacities);
                var stateKey = (i, capKey);
                if (!memo.ContainsKey(stateKey))
                    break;
                int decision = memo[stateKey].decision;
                if (decision != -1)
                {
                    // Проект i назначается команде с индексом decision.
                    assignments[_projects[i].Id] = _teams[decision].Id;
                    // Обновляем оставшиеся дни для команды decision.
                    int duration = 3 + (int)Math.Ceiling((double)_projects[i].T / _teams[decision].Efficiency);
                    capacities[decision] -= duration;
                }
                // В любом случае, переходим к следующему проекту.
            }
            return assignments;
        }

        // Метод Solve вычисляет оптимальное значение и восстанавливает назначение.
        // Итоговая чистая прибыль = (оптимальная сумма profit) - (сумма штрафов по всем проектам).
        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            int[] initialCapacities = Enumerable.Repeat(_quarterDays, _teams.Count).ToArray();
            double dpValue = DP(0, initialCapacities);
            // Оптимальное сумма profit = сумма_{i назначен} (Q_i+C_i).
            // Чистая прибыль = dpValue - sum(C_i) по всем проектам.
            double totalPenalty = _projects.Sum(p => p.C);
            double netProfit = dpValue - totalPenalty;

            // Восстанавливаем назначения: для каждого проекта, если назначен, запоминаем, что он идёт к какой команде.
            Dictionary<int, int> assignments = ReconstructAssignments();

            // Для каждой команды сформируем список назначенных проектов в порядке возрастания индекса проекта.
            Dictionary<int, List<ProjectRequest>> teamAssignments = new();
            foreach (var team in _teams)
            {
                teamAssignments[team.Id] = new List<ProjectRequest>();
            }
            // Проекты рассматриваем в порядке их появления (индекс)
            for (int i = 0; i < _projects.Count; i++)
            {
                int projId = _projects[i].Id;
                if (assignments.ContainsKey(projId))
                {
                    int teamId = assignments[projId];
                    teamAssignments[teamId].Add(_projects[i]);
                }
            }

            // Для каждой команды вычисляем временные интервалы (start, end) согласно порядку назначений.
            List<ProjectInWorkResponse> resultSchedule = new();
            foreach (var team in _teams)
            {
                int currentTime = 0;
                if (teamAssignments.ContainsKey(team.Id))
                {
                    foreach (var proj in teamAssignments[team.Id])
                    {
                        int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                        int startTime = currentTime;
                        int endTime = currentTime + duration;
                        if (endTime <= _quarterDays)
                        {
                            resultSchedule.Add(new ProjectInWorkResponse(proj.Id, team.Id, startTime, endTime));
                            currentTime = endTime;
                        }
                        else break; // если проект не укладывается, последующие не выполняются
                    }
                }
            }

            return new SolutionResponse<ProjectInWorkResponse>(resultSchedule, netProfit);
        }
    }
}
