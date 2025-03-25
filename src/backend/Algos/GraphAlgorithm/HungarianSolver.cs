using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class HungarianSolver<T>
    {
        private readonly BipartiteGraphInput<T> _graphInput;
        private readonly IEqualityComparer<T> _comparer;
        // INF – достаточно большое число, используемое для отсутствующих ребер.
        private const double INF = 1e9;

        // Конструктор сохраняет входные данные и (опционально) компаратор.
        public HungarianSolver(BipartiteGraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _graphInput = graphInput;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        // Метод Solve выполняет венгерский алгоритм и возвращает оптимальное сопоставление
        // в виде record SolutionResponse<MatchingPair<T>>.
        public SolutionResponse<MatchingPair<T>> Solve()
        {
            // Получаем списки левого и правого множеств.
            var left = _graphInput.Left.ToList();
            var right = _graphInput.Right.ToList();
            int nLeft = left.Count;
            int nRight = right.Count;
            // Для решения венгерского алгоритма требуется квадратная матрица.
            int n = Math.Max(nLeft, nRight);

            // Формируем матрицу стоимостей [n x n].
            // Если ребро между элементом left[i] и right[j] отсутствует, задаём стоимость INF.
            double[,] cost = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    cost[i, j] = INF;
                }
            }

            // Строим словари для быстрого определения индекса элемента в left и right.
            var leftIndex = new Dictionary<T, int>(_comparer);
            for (int i = 0; i < nLeft; i++)
            {
                leftIndex[left[i]] = i;
            }
            var rightIndex = new Dictionary<T, int>(_comparer);
            for (int j = 0; j < nRight; j++)
            {
                rightIndex[right[j]] = j;
            }

            // Заполняем матрицу стоимостей по заданным ребрам.
            // Предполагается, что ребра задаются из left в right.
            foreach (var edge in _graphInput.Edges)
            {
                if (leftIndex.TryGetValue(edge.Source, out int i) &&
                    rightIndex.TryGetValue(edge.Target, out int j))
                {
                    cost[i, j] = edge.Weight;
                }
            }

            // Выполняем венгерский алгоритм, который возвращает массив assignment:
            // assignment[i] = номер столбца (из правого множества), назначенный строке i (из левого).
            int[] assignment = HungarianAlgorithm(cost, n);

            // Формируем результирующее сопоставление и суммарную стоимость.
            var matching = new List<MatchingPair<T>>();
            double totalCost = 0;
            // Рассматриваем только i < nLeft и assignment[i] < nRight, то есть реальные элементы.
            for (int i = 0; i < nLeft; i++)
            {
                int j = assignment[i];
                if (j < nRight && cost[i, j] < INF)
                {
                    matching.Add(new MatchingPair<T>(left[i], right[j]));
                    totalCost += cost[i, j];
                }
            }

            return new SolutionResponse<MatchingPair<T>>(matching, totalCost);
        }

        // Реализация венгерского алгоритма (Hungarian algorithm) для минимизации стоимости.
        // Работает с квадратной матрицей размера n x n.
        // Возвращает массив assignment, где assignment[i] – индекс столбца, назначенный строке i.
        private int[] HungarianAlgorithm(double[,] cost, int n)
        {
            double[] u = new double[n + 1];
            double[] v = new double[n + 1];
            int[] p = new int[n + 1];
            int[] way = new int[n + 1];

            for (int i = 1; i <= n; i++)
            {
                p[0] = i;
                double[] minv = new double[n + 1];
                bool[] used = new bool[n + 1];
                for (int j = 0; j <= n; j++)
                {
                    minv[j] = INF;
                    used[j] = false;
                }
                int j0 = 0;
                do
                {
                    used[j0] = true;
                    int i0 = p[j0];
                    int j1 = 0;
                    double delta = INF;
                    for (int j = 1; j <= n; j++)
                    {
                        if (!used[j])
                        {
                            double cur = cost[i0 - 1, j - 1] - u[i0] - v[j];
                            if (cur < minv[j])
                            {
                                minv[j] = cur;
                                way[j] = j0;
                            }
                            if (minv[j] < delta)
                            {
                                delta = minv[j];
                                j1 = j;
                            }
                        }
                    }
                    for (int j = 0; j <= n; j++)
                    {
                        if (used[j])
                        {
                            u[p[j]] += delta;
                            v[j] -= delta;
                        }
                        else
                        {
                            minv[j] -= delta;
                        }
                    }
                    j0 = j1;
                } while (p[j0] != 0);
                do
                {
                    int j1 = way[j0];
                    p[j0] = p[j1];
                    j0 = j1;
                } while (j0 != 0);
            }

            int[] ans = new int[n];
            for (int j = 1; j <= n; j++)
            {
                if (p[j] != 0)
                {
                    ans[p[j] - 1] = j - 1;
                }
            }
            return ans;
        }
    }
}
