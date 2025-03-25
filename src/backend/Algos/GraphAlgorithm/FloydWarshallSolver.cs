using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;
using System.Linq;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class FloydWarshallSolver<T>
    {
        private readonly GraphInput<T> _graphInput;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор сохраняет входные данные и компаратор, без выполнения вычислений
        public FloydWarshallSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _graphInput = graphInput;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        // Метод Solve выполняет все вычисления алгоритма Флойда–Уоршелла и возвращает результат
        public SolutionResponse<T> Solve()
        {
            // Инициализируем список узлов и ребер
            var nodes = new List<T>(_graphInput.Nodes);
            var edges = new List<Edge<T>>(_graphInput.Edges);
            int n = nodes.Count;

            // Инициализация матрицы расстояний и матрицы для восстановления пути
            var dist = new double[n, n];
            var next = new T?[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        dist[i, j] = 0;
                    else
                        dist[i, j] = double.PositiveInfinity;

                    next[i, j] = default;
                }
            }

            // Заполнение матрицы расстояний для заданных ребер
            foreach (var edge in edges)
            {
                int i = nodes.IndexOf(edge.Source);
                int j = nodes.IndexOf(edge.Target);
                if (i == -1 || j == -1)
                    continue;
                if (edge.Weight < dist[i, j])
                {
                    dist[i, j] = edge.Weight;
                    next[i, j] = edge.Target;
                }
            }

            // Основной цикл алгоритма Флойда–Уоршелла: перебор всех пар вершин через промежуточную вершину k
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[i, k] + dist[k, j] < dist[i, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }

            // Проверка на отрицательные циклы: если на диагонали матрицы встречается значение меньше 0, такой цикл имеется
            for (int i = 0; i < n; i++)
            {
                if (dist[i, i] < 0)
                    throw new InvalidOperationException("Граф содержит отрицательный цикл");
            }

            // Восстановление пути от _graphInput.Start до _graphInput.Target
            int startIndex = nodes.IndexOf(_graphInput.Start);
            int targetIndex = nodes.IndexOf(_graphInput.Target);
            var path = ReconstructPath(startIndex, targetIndex, nodes, dist, next);

            double score = dist[startIndex, targetIndex];

            return new SolutionResponse<T>(path, score);
        }

        // Метод восстановления пути от вершины с индексом u до вершины с индексом v
        private List<T> ReconstructPath(int u, int v, List<T> nodes, double[,] dist, T?[,] next)
        {
            var path = new List<T>();

            if (double.IsPositiveInfinity(dist[u, v]))
                return path; // Пути нет

            path.Add(nodes[u]);
            while (u != v)
            {
                T? nextNode = next[u, v];
                if (nextNode == null)
                    return new List<T>(); // Пути нет
                u = nodes.IndexOf(nextNode);
                path.Add(nextNode);
            }
            return path;
        }
    }
}
