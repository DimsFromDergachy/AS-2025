using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.TasksSchedule
{
    public class BranchAndBoundScheduler
    {
        private readonly List<TeamRequest> _teams;
        private readonly List<ProjectRequest> _projects;
        private readonly int _quarterDays; // длительность квартала (например, 90 дней)
                                           // Для упрощения алгоритма проекты рассматриваются в порядке списка _projects.
                                           // Предварительно вычисляем потенциал для оставшихся проектов: для проекта p потенциал = (Q + C)
        private readonly double[] _remainingPotential;

        private double _bestScore = double.NegativeInfinity;
        private ScheduleSolution? _bestSolution = null;

        public BranchAndBoundScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays)
        {
            _teams = teams;
            _projects = projects;
            _quarterDays = quarterDays;

            int n = _projects.Count;
            _remainingPotential = new double[n + 1];
            // _remainingPotential[i] = сумма для k от i до n-1 (proj[k].Q + proj[k].C)
            _remainingPotential[n] = 0;
            for (int i = n - 1; i >= 0; i--)
            {
                _remainingPotential[i] = _remainingPotential[i + 1] + (_projects[i].Q + _projects[i].C);
            }
        }

        // Основной метод, запускающий рекурсивный перебор
        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            // Начальное состояние: ни один проект не назначен
            ScheduleSolution initial = new ScheduleSolution(_teams, _projects);
            // Начальный результат (если ничего не назначить, все проекты не выполнены, чистая прибыль = -сумма штрафов)
            double initScore = Evaluate(initial);
            _bestScore = initScore;
            _bestSolution = initial.DeepCopy();

            Branch(0, initial);

            return ConvertToResult(_bestScore, _bestSolution!);
        }

        // Рекурсивная функция перебора
        // index - номер проекта из _projects, который сейчас рассматривается.
        private void Branch(int index, ScheduleSolution state)
        {
            int n = _projects.Count;
            if (index == n)
            {
                double score = Evaluate(state);
                if (score > _bestScore)
                {
                    _bestScore = score;
                    _bestSolution = state.DeepCopy();
                }
                return;
            }

            // Вычисляем текущую оценку (на основе всех проектов: если проект не назначен – штраф, если назначен и укладывается – прибыль)
            double currentScore = Evaluate(state);
            // Верхняя граница для данной ветви:
            // Предполагаем, что для оставшихся проектов (с индексом >= index) удастся получить максимум (Q + C) каждый.
            double bound = currentScore + _remainingPotential[index];
            if (bound < _bestScore)
                return; // Отсекаем ветвь

            ProjectRequest proj = _projects[index];

            // Вариант 1: не назначать проект (оставить его в списке Unscheduled)
            // (Если проект уже не в списке Unscheduled, значит он был назначен ранее; однако, поскольку каждый проект рассматривается один раз, он должен быть там)
            // Создаем копию состояния и переходим к следующему проекту.
            ScheduleSolution stateNoAssign = state.DeepCopy();
            // Убедимся, что проект присутствует в Unscheduled (если его там нет, значит он уже назначен, и этот вариант уже рассмотрен)
            if (stateNoAssign.Unscheduled.Any(p => p.Id == proj.Id))
                stateNoAssign.Unscheduled.RemoveAll(p => p.Id == proj.Id);
            // Поскольку не назначили – остаётся штраф за этот проект (в Evaluate он учтется как не выполненный)
            Branch(index + 1, stateNoAssign);

            // Вариант 2: назначить проект одной из команд.
            foreach (var team in _teams)
            {
                ScheduleSolution stateAssign = state.DeepCopy();
                // Если проект уже был назначен (возможен, если мы выбираем вариант "не назначать" в другом варианте) – пропускаем
                if (!stateAssign.Unscheduled.Any(p => p.Id == proj.Id))
                    continue;
                stateAssign.Unscheduled.RemoveAll(p => p.Id == proj.Id);
                // Добавляем проект в конец расписания команды
                stateAssign.TeamSchedules[team.Id].Add(proj);
                Branch(index + 1, stateAssign);
            }
        }

        // Функция оценки решения, аналогичная Evaluate из предыдущих примеров:
        // Для каждой команды последовательно суммируем время выполнения проектов;
        // если проект укладывается в _quarterDays, считаем его выполненным (прибавляем Q), иначе – не выполненным (штраф -C).
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
                            break; // последующие проекты в этой команде не выполняются
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

        // Преобразование конечного решения в итоговый список назначений с вычислением start/end для каждого проекта
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
