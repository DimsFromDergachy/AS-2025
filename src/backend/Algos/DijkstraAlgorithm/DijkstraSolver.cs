using AS_2025.Algos.Common;
using AS_2025.Algos.DijkstraAlgorithm.Models;

namespace AS_2025.Algos.DijkstraAlgorithm
{
    public class DijkstraSolver<T>
    {
        private readonly T _start;
        private readonly T _target;
        private readonly Func<T, IEnumerable<(T neighbor, double weight)>> _getNeighbors;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор получает входные данные графа и компаратор (если требуется)
        public DijkstraSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _start = graphInput.Start;
            _target = graphInput.Target;

            // Формируем представление графа в виде словаря: для каждого узла получаем список соседей и вес ребра.
            var graphDict = new Dictionary<T, List<(T neighbor, double weight)>>(_comparer);
            foreach (var node in graphInput.Nodes)
            {
                graphDict[node] = new List<(T, double)>();
            }
            foreach (var edge in graphInput.Edges)
            {
                // Для ориентированного графа: добавляем ребро от Source к Target.
                graphDict[edge.Source].Add((edge.Target, edge.Weight));
            }

            // Функция для получения соседей узла
            _getNeighbors = node => graphDict[node];
        }

        // Метод, реализующий алгоритм Дейкстры, и возвращающий результат в виде SolutionResponse<T>
        public SolutionResponse<T> Solve()
        {
            var distances = new Dictionary<T, double>(_comparer);
            var previous = new Dictionary<T, T>(_comparer);
            var queue = new PriorityQueue<T, double>();

            distances[_start] = 0;
            queue.Enqueue(_start, 0);

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                if (_comparer.Equals(current, _target))
                    break;

                double currentDistance = distances[current];

                foreach (var (neighbor, weight) in _getNeighbors(current))
                {
                    double newDistance = currentDistance + weight;
                    if (!distances.ContainsKey(neighbor) || newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        previous[neighbor] = current;
                        queue.Enqueue(neighbor, newDistance);
                    }
                }
            }

            // Если целевой узел недостижим, возвращаем пустой путь с бесконечной стоимостью.
            if (!distances.ContainsKey(_target))
                return new SolutionResponse<T>(new List<T>(), double.PositiveInfinity);

            // Восстанавливаем путь от _target к _start через словарь previous.
            var path = new List<T>();
            T currentNode = _target;
            while (!_comparer.Equals(currentNode, _start))
            {
                path.Add(currentNode);
                currentNode = previous[currentNode];
            }
            path.Add(_start);
            path.Reverse();

            return new SolutionResponse<T>(path, distances[_target]);
        }
    }
}
