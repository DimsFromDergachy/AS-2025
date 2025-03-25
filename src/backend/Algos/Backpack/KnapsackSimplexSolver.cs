using AS_2025.Algos.Common;
using Google.OrTools.LinearSolver;

namespace AS_2025.Algos.Backpack
{
    public class KnapsackSimplexSolver
    {
        private readonly int _capacity;
        private readonly List<Item> _items;

        public KnapsackSimplexSolver(int capacity, List<Item> items)
        {
            _capacity = capacity;
            _items = items;
        }

        public SolutionResponse<Item> Solve()
        {
            // Создаем MILP-решатель (CBC)
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
            if (solver == null)
                throw new Exception("Solver not created.");

            int n = _items.Count;

            // Создаем бинарные переменные: x[i] = 1, если предмет i выбран, иначе 0.
            Variable[] x = new Variable[n];
            for (int i = 0; i < n; i++)
            {
                x[i] = solver.MakeBoolVar($"x_{i}");
            }

            // Ограничение по весу: суммарный вес выбранных предметов <= _capacity.
            // Вместо LinearExpr.Sum создадим сумму вручную.
            LinearExpr weightExpr = x[0] * _items[0].Weight;
            for (int i = 1; i < n; i++)
            {
                weightExpr = weightExpr + x[i] * _items[i].Weight;
            }
            solver.Add(weightExpr <= _capacity);

            // Целевая функция: максимизировать суммарную ценность выбранных предметов.
            LinearExpr objective = x[0] * _items[0].Value;
            for (int i = 1; i < n; i++)
            {
                objective = objective + x[i] * _items[i].Value;
            }
            solver.Maximize(objective);

            // Решаем модель.
            Solver.ResultStatus resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL &&
                resultStatus != Solver.ResultStatus.FEASIBLE)
            {
                throw new Exception("Solution not found.");
            }

            // Извлекаем решение.
            List<Item> selectedItems = new();
            double objectiveValue = solver.Objective().Value();
            for (int i = 0; i < n; i++)
            {
                if (x[i].SolutionValue() > 0.5)
                {
                    selectedItems.Add(_items[i]);
                }
            }

            return new SolutionResponse<Item>(selectedItems, objectiveValue);
        }
    }
}
