using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class FordFulkersonSolver<T>
    {
        private readonly GraphInput<T> _graphInput;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор сохраняет входные данные и компаратор (если требуется)
        public FordFulkersonSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _graphInput = graphInput;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        // Метод Solve выполняет алгоритм Форда–Фалкерсона и возвращает результат:
        // список ребер с назначенными потоками и значение максимального потока.
        public SolutionResponse<FlowEdge<T>> Solve()
        {
            // Получаем список узлов из входных данных
            var nodes = new List<T>(_graphInput.Nodes);

            // Инициализируем остаточные пропускные способности и потоки для каждой пары вершин.
            // Для каждой вершины u создаём словарь, где ключ — соседняя вершина v, а значение — остаточная способность (и изначально поток = 0).
            var residual = new Dictionary<T, Dictionary<T, double>>(_comparer);
            var flows = new Dictionary<T, Dictionary<T, double>>(_comparer);
            foreach (var u in nodes)
            {
                residual[u] = new Dictionary<T, double>(_comparer);
                flows[u] = new Dictionary<T, double>(_comparer);
                foreach (var v in nodes)
                {
                    residual[u][v] = 0;
                    flows[u][v] = 0;
                }
            }

            // Заполняем начальные остаточные пропускные способности по входным ребрам.
            // Используем поле Weight как пропускную способность.
            foreach (var edge in _graphInput.Edges)
            {
                residual[edge.Source][edge.Target] += edge.Weight;
                // Обратное ребро изначально имеет пропускную способность 0.
            }

            double maxFlow = 0;
            T source = _graphInput.Start;
            T sink = _graphInput.Target;

            // Основной цикл: ищем аугментирующие пути в остаточном графе.
            while (true)
            {
                // parent хранит для каждой вершины предыдущую вершину на найденном пути.
                var parent = new Dictionary<T, T>(_comparer);
                var visited = new HashSet<T>(_comparer);
                bool pathFound = DFS(source, sink, residual, parent, visited);
                if (!pathFound)
                    break;

                // Определяем bottleneck — минимальную остаточную способность вдоль найденного пути.
                double bottleneck = double.PositiveInfinity;
                T v = sink;
                while (!_comparer.Equals(v, source))
                {
                    T u = parent[v];
                    bottleneck = Math.Min(bottleneck, residual[u][v]);
                    v = u;
                }

                // Обновляем остаточные пропускные способности и потоки вдоль найденного пути.
                v = sink;
                while (!_comparer.Equals(v, source))
                {
                    T u = parent[v];
                    residual[u][v] -= bottleneck;
                    residual[v][u] += bottleneck;
                    flows[u][v] += bottleneck;
                    v = u;
                }

                maxFlow += bottleneck;
            }

            // Формируем список FlowEdge по исходным ребрам.
            var flowEdges = new List<FlowEdge<T>>();
            foreach (var edge in _graphInput.Edges)
            {
                double flow = flows[edge.Source][edge.Target];
                flowEdges.Add(new FlowEdge<T>(edge.Source, edge.Target, flow, edge.Weight));
            }

            return new SolutionResponse<FlowEdge<T>>(flowEdges, maxFlow);
        }

        // Метод DFS ищет аугментирующий путь в остаточном графе.
        // Если путь найден, parent заполняется, и возвращается true.
        private bool DFS(T current, T sink, Dictionary<T, Dictionary<T, double>> residual, Dictionary<T, T> parent, HashSet<T> visited)
        {
            visited.Add(current);
            if (_comparer.Equals(current, sink))
                return true;

            foreach (var kvp in residual[current])
            {
                T neighbor = kvp.Key;
                double capacity = kvp.Value;
                if (capacity > 0 && !visited.Contains(neighbor))
                {
                    parent[neighbor] = current;
                    if (DFS(neighbor, sink, residual, parent, visited))
                        return true;
                }
            }
            return false;
        }
    }
}
