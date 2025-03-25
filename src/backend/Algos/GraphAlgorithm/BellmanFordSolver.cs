using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class BellmanFordSolver<T>
    {
        private readonly T _start;
        private readonly T _target;
        private readonly List<T> _nodes;
        private readonly List<Edge<T>> _edges;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор получает входные данные графа и компаратор (если требуется)
        public BellmanFordSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _start = graphInput.Start;
            _target = graphInput.Target;
            _nodes = new List<T>(graphInput.Nodes);
            _edges = new List<Edge<T>>(graphInput.Edges);
        }

        // Метод, реализующий алгоритм Беллмана-Форда
        public SolutionResponse<T> Solve()
        {
            // Инициализация расстояний: для всех узлов расстояние = +∞, для стартового узла = 0
            var distances = new Dictionary<T, double>(_comparer);
            var previous = new Dictionary<T, T>(_comparer);

            foreach (var node in _nodes)
            {
                distances[node] = double.PositiveInfinity;
            }
            distances[_start] = 0;

            // Релаксация ребер (|V| - 1) раз
            int vertexCount = _nodes.Count;
            for (int i = 1; i <= vertexCount - 1; i++)
            {
                foreach (var edge in _edges)
                {
                    if (distances[edge.Source] + edge.Weight < distances[edge.Target])
                    {
                        distances[edge.Target] = distances[edge.Source] + edge.Weight;
                        previous[edge.Target] = edge.Source;
                    }
                }
            }

            // Проверка на отрицательные циклы
            foreach (var edge in _edges)
            {
                if (distances[edge.Source] + edge.Weight < distances[edge.Target])
                    throw new InvalidOperationException("Граф содержит отрицательный весовой цикл");
            }

            // Если целевой узел недостижим, возвращаем пустой путь и бесконечную стоимость.
            if (double.IsPositiveInfinity(distances[_target]))
                return new SolutionResponse<T>(new List<T>(), double.PositiveInfinity);

            // Восстанавливаем путь от целевого узла к стартовому
            var path = new List<T>();
            T current = _target;
            while (!_comparer.Equals(current, _start))
            {
                path.Add(current);
                current = previous[current];
            }
            path.Add(_start);
            path.Reverse();

            return new SolutionResponse<T>(path, distances[_target]);
        }
    }
}
