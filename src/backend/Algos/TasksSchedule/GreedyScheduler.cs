using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule
{
    public class GreedyScheduler
    {
        private List<TeamRequest> _teams;
        private List<ProjectRequest> _projects;
        private int _quarterDays; // Длительность квартала (например, 90 дней)

        public GreedyScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays)
        {
            _teams = teams;
            _projects = projects;
            this._quarterDays = quarterDays;
        }

        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            // Для каждой команды отслеживаем текущее время (конец последнего назначенного проекта)
            Dictionary<int, int> teamCurrentTime = new Dictionary<int, int>();
            // Для каждой команды сохраняем список назначений
            Dictionary<int, List<ProjectInWorkResponse>> teamSchedules = new Dictionary<int, List<ProjectInWorkResponse>>();
            foreach (var team in _teams)
            {
                teamCurrentTime[team.Id] = 0;
                teamSchedules[team.Id] = new List<ProjectInWorkResponse>();
            }

            // Вычисляем для каждого проекта минимальную длительность выполнения среди всех команд,
            // а также метрику плотности: (q_i + c_i) / duration.
            var projectMetrics = new List<(ProjectRequest proj, double density)>();
            foreach (var proj in _projects)
            {
                int bestDuration = int.MaxValue;
                foreach (var team in _teams)
                {
                    // Длительность для данной команды: 3 дня на вникание + время выполнения (округление вверх)
                    int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                    if (duration < bestDuration)
                        bestDuration = duration;
                }
                double density = (proj.Q + proj.C) / (double)bestDuration;
                projectMetrics.Add((proj, density));
            }

            // Сортируем проекты по убыванию плотности
            var sortedProjects = projectMetrics.OrderByDescending(x => x.density)
                                                .Select(x => x.proj)
                                                .ToList();

            // Отмечаем проекты, которые удалось назначить (и завершить в срок)
            HashSet<int> scheduledProjects = new HashSet<int>();

            // Жадное назначение: для каждого проекта пытаемся найти команду, которая сможет выполнить его раньше всех
            foreach (var proj in sortedProjects)
            {
                int bestTeamId = -1;
                int bestFinishTime = int.MaxValue;
                int bestStartTime = -1;

                foreach (var team in _teams)
                {
                    int currentTime = teamCurrentTime[team.Id];
                    int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                    int finishTime = currentTime + duration;
                    // Если проект укладывается в сроки квартала
                    if (finishTime <= _quarterDays && finishTime < bestFinishTime)
                    {
                        bestTeamId = team.Id;
                        bestFinishTime = finishTime;
                        bestStartTime = currentTime;
                    }
                }

                if (bestTeamId != -1)
                {
                    // Назначаем проект выбранной команде
                    teamSchedules[bestTeamId].Add(new ProjectInWorkResponse(proj.Id, bestTeamId, bestStartTime, bestFinishTime));
                    teamCurrentTime[bestTeamId] = bestFinishTime;
                    scheduledProjects.Add(proj.Id);
                }
                // Если ни одна команда не может выполнить проект в срок – проект остаётся нераспределённым.
            }

            // Объединяем расписания всех команд в один список
            List<ProjectInWorkResponse> resultSchedule = new List<ProjectInWorkResponse>();
            foreach (var team in _teams)
            {
                resultSchedule.AddRange(teamSchedules[team.Id]);
            }

            // Вычисляем чистую прибыль:
            // Для каждого реализованного проекта прибавляем q_i,
            // для нереализованного (не назначенного ни одной команде) вычитаем c_i.
            double netProfit = 0;
            foreach (var proj in _projects)
            {
                if (scheduledProjects.Contains(proj.Id))
                    netProfit += proj.Q;
                else
                    netProfit -= proj.C;
            }

            return new SolutionResponse<ProjectInWorkResponse>(resultSchedule, netProfit);
        }
    }

}
