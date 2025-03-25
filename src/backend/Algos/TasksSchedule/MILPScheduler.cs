using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;
using Google.OrTools.LinearSolver;

namespace AS_2025.Algos.TasksSchedule
{
    public class MILPScheduler
    {
        private readonly List<TeamRequest> _teams;
        private readonly List<ProjectRequest> _projects;
        private readonly int _quarterDays; // D, например 90 дней

        public MILPScheduler(List<TeamRequest> teams, List<ProjectRequest> projects, int quarterDays)
        {
            _teams = teams;
            _projects = projects;
            _quarterDays = quarterDays;
        }

        public SolutionResponse<ProjectInWorkResponse> Solve()
        {
            // Создаем MILP-решатель (CBC)
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
            if (solver == null) throw new Exception("Solver not created.");

            int M = _projects.Count;
            int N = _teams.Count;

            // Большое число для big-M
            double BigM = _quarterDays + 100;

            // Предварительно вычисляем p[i,j] = 3 + ceil(T_i / Efficiency_j)
            int[,] p = new int[M, N];
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    p[i, j] = 3 + (int)Math.Ceiling((double)_projects[i].T / _teams[j].Efficiency);
                }
            }

            // Переменные:
            // x[i,j] бинарная: 1, если проект i назначен команде j.
            Variable[,] x = new Variable[M, N];
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    x[i, j] = solver.MakeBoolVar($"x_{i}_{j}");

            // s[i] - время начала проекта i, f[i] - время окончания проекта i.
            Variable[] s = new Variable[M];
            Variable[] f = new Variable[M];
            for (int i = 0; i < M; i++)
            {
                s[i] = solver.MakeNumVar(0, _quarterDays, $"s_{i}");
                f[i] = solver.MakeNumVar(0, _quarterDays, $"f_{i}");
            }

            // u[i] бинарная: 1, если проект i завершен в срок.
            Variable[] u = new Variable[M];
            for (int i = 0; i < M; i++)
                u[i] = solver.MakeBoolVar($"u_{i}");

            // Для упорядочивания проектов на одной команде:
            // Для каждой команды j и для каждой пары проектов (i,k) с i < k, вводим бинарную переменную y[i,k,j]
            // которая равна 1, если проект i выполняется до проекта k на команде j.
            var y = new Dictionary<(int, int, int), Variable>();
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < M; i++)
                {
                    for (int k = i + 1; k < M; k++)
                    {
                        y[(i, k, j)] = solver.MakeBoolVar($"y_{i}_{k}_{j}");

                        // Если оба проекта назначены команде j, то должны соблюдаться
                        // один из двух порядков:
                        // s[i] + p[i,j] <= s[k] + BigM*(1 - y[i,k,j])
                        // s[k] + p[k,j] <= s[i] + BigM*y[i,k,j]
                        // Чтобы эти ограничения действовали только если x[i,j] = x[k,j] = 1, добавляем мультипликаторы.
                        solver.Add(s[i] + p[i, j] <= s[k] + BigM * (1 - y[(i, k, j)]) + BigM * (2 - x[i, j] - x[k, j]));
                        solver.Add(s[k] + p[k, j] <= s[i] + BigM * y[(i, k, j)] + BigM * (2 - x[i, j] - x[k, j]));
                    }
                }
            }

            var constVar = solver.MakeNumVar(1, 1, "const");

            // Ограничение: каждый проект назначается не более чем одной команде.
            for (int i = 0; i < M; i++)
            {
                LinearExpr assignSum = 0.0 * constVar;
                for (int j = 0; j < N; j++)
                    assignSum += x[i, j];
                solver.Add(assignSum <= 1);
            }

            // Определяем время окончания проекта: f[i] = s[i] + sum_{j} p[i,j] * x[i,j]
            for (int i = 0; i < M; i++)
            {
                LinearExpr procTime = 0.0 * constVar;
                for (int j = 0; j < N; j++)
                    procTime += p[i, j] * x[i, j];
                solver.Add(f[i] == s[i] + procTime);
            }

            // Если проект завершен (u[i]=1), то f[i] <= _quarterDays.
            for (int i = 0; i < M; i++)
                solver.Add(f[i] <= _quarterDays + BigM * (1 - u[i]));

            // Если проект не назначен, то u[i] должна быть 0.
            for (int i = 0; i < M; i++)
            {
                LinearExpr assignSum = 0.0 * constVar;
                for (int j = 0; j < N; j++)
                    assignSum += x[i, j];
                solver.Add(u[i] <= assignSum);
            }

            // Целевая функция: максимизировать sum_{i}( (q_i + c_i)*u[i] ).
            // Чистая прибыль будет равна этой суммы минус сумма штрафов по всем проектам.
            LinearExpr objective = 0.0 * constVar;
            for (int i = 0; i < M; i++)
                objective += (_projects[i].Q + _projects[i].C) * u[i];
            solver.Maximize(objective);

            // Решаем модель.
            Solver.ResultStatus resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL && resultStatus != Solver.ResultStatus.FEASIBLE)
                throw new Exception("No solution found.");

            double optimalObjective = solver.Objective().Value();
            double totalPenalty = _projects.Sum(pj => pj.C);
            double netProfit = optimalObjective - totalPenalty;

            // Восстанавливаем расписание: для каждого проекта, определяем, к какой команде он назначен, и читаем s[i] и f[i].
            // Группируем по командам.
            var teamProjects = new Dictionary<int, List<(int projIndex, double start, double finish)>>();
            for (int j = 0; j < N; j++)
                teamProjects[_teams[j].Id] = new List<(int, double, double)>();

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (x[i, j].SolutionValue() > 0.5)
                    {
                        teamProjects[_teams[j].Id].Add((i, s[i].SolutionValue(), f[i].SolutionValue()));
                    }
                }
            }

            // Для каждой команды сортируем проекты по времени начала.
            List<ProjectInWorkResponse> assignments = new List<ProjectInWorkResponse>();
            foreach (var team in _teams)
            {
                var projList = teamProjects[team.Id].OrderBy(item => item.start).ToList();
                int currentTime = 0;
                foreach (var (projIndex, start, finish) in projList)
                {
                    int duration = p[projIndex, _teams.FindIndex(t => t.Id == team.Id)];
                    int st = currentTime;
                    int en = currentTime + duration;
                    if (en <= _quarterDays)
                    {
                        assignments.Add(new ProjectInWorkResponse(_projects[projIndex].Id, team.Id, st, en));
                        currentTime = en;
                    }
                }
            }

            return new SolutionResponse<ProjectInWorkResponse>(assignments, netProfit);
        }
    }
}
