using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class PrimSolver<T>
    {
        private readonly GraphInput<T> _graphInput;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор сохраняет входные данные и компаратор (если требуется)
        public PrimSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _graphInput = graphInput;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        // Метод Solve выполняет алгоритм Прима и возвращает минимальное остовное дерево в виде списка ребер и суммарный вес.
        public SolutionResponse<Edge<T>> Solve()
        {
            // Инициализируем список узлов и строим представление графа (аджacency list)
            var nodes = new List<T>(_graphInput.Nodes);
            int totalNodes = nodes.Count;

            // Создаём словарь смежности: для каждого узла хранится список ребер (учитываем двунаправленность)
            var adjacencyList = new Dictionary<T, List<Edge<T>>>(_comparer);
            foreach (var node in nodes)
            {
                adjacencyList[node] = new List<Edge<T>>();
            }
            foreach (var edge in _graphInput.Edges)
            {
                // Для неориентированного графа добавляем ребро в обе стороны
                if (adjacencyList.ContainsKey(edge.Source))
                    adjacencyList[edge.Source].Add(edge);
                if (adjacencyList.ContainsKey(edge.Target))
                    adjacencyList[edge.Target].Add(new Edge<T>(edge.Target, edge.Source, edge.Weight));
            }

            // Множество вершин, уже включённых в MST
            var inMST = new HashSet<T>(_comparer);
            // Результирующий список ребер MST
            var mstEdges = new List<Edge<T>>();
            double totalWeight = 0;

            // Выбираем стартовую вершину (из входных данных)
            T start = _graphInput.Start;
            inMST.Add(start);

            // Очередь с приоритетами для ребер (минимальный вес имеет больший приоритет)
            var edgeQueue = new PriorityQueue<Edge<T>, double>();

            // Добавляем все ребра, исходящие из стартовой вершины, в очередь
            foreach (var edge in adjacencyList[start])
            {
                edgeQueue.Enqueue(edge, edge.Weight);
            }

            // Пока MST не содержит все узлы и очередь не пуста
            while (inMST.Count < totalNodes && edgeQueue.Count > 0)
            {
                // Извлекаем ребро минимального веса
                Edge<T> minEdge = edgeQueue.Dequeue();

                // Если конечная вершина ребра уже включена – пропускаем его
                if (inMST.Contains(minEdge.Target))
                    continue;

                // Добавляем ребро в MST
                mstEdges.Add(minEdge);
                totalWeight += minEdge.Weight;
                T newVertex = minEdge.Target;
                inMST.Add(newVertex);

                // Добавляем ребра, исходящие из новой вершины, в очередь
                foreach (var edge in adjacencyList[newVertex])
                {
                    if (!inMST.Contains(edge.Target))
                    {
                        edgeQueue.Enqueue(edge, edge.Weight);
                    }
                }
            }

            // Если MST не содержит все узлы, граф несвязный.
            if (inMST.Count < totalNodes)
                throw new InvalidOperationException("Граф несвязный, MST не может покрыть все вершины.");

            return new SolutionResponse<Edge<T>>(mstEdges, totalWeight);
        }
    }
}
