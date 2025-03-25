using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class MinimumCutSolver<T>
    {
        private readonly GraphInput<T> _graphInput;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор сохраняет входные данные и компаратор (если требуется).
        // Поля Start и Target игнорируются.
        public MinimumCutSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _graphInput = graphInput;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        // Метод Solve выполняет алгоритм Стойера–Уэллингера (Stoer–Wagner)
        // и возвращает минимальный разрез в виде:
        // - Solution: список вершин, принадлежащих одной из частей разбиения (одна сторона разреза)
        // - Score: суммарный вес разреза
        public SolutionResponse<T> Solve()
        {
            // Строим представление графа в виде матрицы смежности (словарь вершин -> (вершина -> суммарный вес ребра))
            var nodes = new List<T>(_graphInput.Nodes);
            int n = nodes.Count;
            var graph = new Dictionary<T, Dictionary<T, double>>(_comparer);
            foreach (var v in nodes)
            {
                graph[v] = new Dictionary<T, double>(_comparer);
                // Для всех других вершин задаём начальное значение 0
                foreach (var u in nodes)
                {
                    if (!_comparer.Equals(u, v))
                        graph[v][u] = 0;
                }
            }

            // Заполняем матрицу по входным ребрам (так как граф неориентированный, обновляем обе записи)
            foreach (var edge in _graphInput.Edges)
            {
                if (!graph.ContainsKey(edge.Source) || !graph.ContainsKey(edge.Target))
                    continue;
                graph[edge.Source][edge.Target] += edge.Weight;
                graph[edge.Target][edge.Source] += edge.Weight;
            }

            // Для поддержки слияния вершин будем хранить, какие исходные вершины входят в текущую "супервершину"
            var vertexSets = new Dictionary<T, HashSet<T>>(_comparer);
            foreach (var v in nodes)
            {
                vertexSets[v] = new HashSet<T>(_comparer) { v };
            }

            double bestCut = double.PositiveInfinity;
            HashSet<T>? bestPartition = null;

            // Пока в графе больше одной вершины выполняем фазу алгоритма
            while (graph.Count > 1)
            {
                // Фаза Стойера–Уэллингера:
                // 1. Инициализируем массив "weights" и множество добавленных вершин "added"
                var currentVertices = new List<T>(graph.Keys);
                var added = new HashSet<T>(_comparer);
                var weights = new Dictionary<T, double>(_comparer);
                foreach (var v in currentVertices)
                {
                    weights[v] = 0;
                }

                T prev = default!;
                T last = default!;
                // 2. Итеративно добавляем вершины, выбирая на каждом шаге вершину с максимальной суммарной связью с уже добавленными.
                for (int i = 0; i < currentVertices.Count; i++)
                {
                    // Выбираем вершину, не включённую в added, с максимальным весом
                    T next = currentVertices
                        .Where(v => !added.Contains(v))
                        .OrderByDescending(v => weights[v])
                        .First();
                    added.Add(next);
                    if (i == currentVertices.Count - 1)
                    {
                        // Последняя добавленная вершина
                        last = next;
                        break;
                    }
                    prev = next;
                    // Обновляем веса для оставшихся вершин
                    foreach (var v in currentVertices)
                    {
                        if (!added.Contains(v))
                        {
                            weights[v] += graph[next][v];
                        }
                    }
                }

                // Веса для последней добавленной вершины задают вес текущего разреза
                double phaseCut = weights[last];
                if (phaseCut < bestCut)
                {
                    bestCut = phaseCut;
                    // В качестве разреза выбираем множество исходных вершин, входящих в супервершину last
                    bestPartition = new HashSet<T>(vertexSets[last], _comparer);
                }

                // Слияние вершин: объединяем предпоследнюю (prev) и последнюю (last)
                // Обновляем веса ребер для объединённой вершины (prev)
                foreach (var v in currentVertices)
                {
                    if (_comparer.Equals(v, prev) || _comparer.Equals(v, last))
                        continue;
                    graph[prev][v] += graph[last][v];
                    graph[v][prev] = graph[prev][v];
                }
                // Объединяем наборы вершин
                vertexSets[prev].UnionWith(vertexSets[last]);

                // Удаляем вершину last из графа и из vertexSets
                graph.Remove(last);
                foreach (var kvp in graph)
                {
                    if (kvp.Value.ContainsKey(last))
                        kvp.Value.Remove(last);
                }
                vertexSets.Remove(last);
            }

            // Если bestPartition не найден (например, пустой граф), возвращаем пустой результат
            var solution = bestPartition != null ? bestPartition.ToList() : new List<T>();
            return new SolutionResponse<T>(solution, bestCut);
        }
    }
}
