using AS_2025.Algos.Common;
using AS_2025.Algos.GraphAlgorithm.Models;

namespace AS_2025.Algos.GraphAlgorithm
{
    public class GraphColoringSolver<T>
    {
        private readonly GraphInput<T> _graphInput;
        private readonly IEqualityComparer<T> _comparer;

        // Конструктор сохраняет входные данные и компаратор
        public GraphColoringSolver(GraphInput<T> graphInput, IEqualityComparer<T>? comparer = null)
        {
            _graphInput = graphInput;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        // Метод Solve выполняет раскраску вершин
        public SolutionResponse<ColoredVertex<T>> Solve()
        {
            // Получаем список узлов и ребер
            var nodes = new List<T>(_graphInput.Nodes);
            var edges = new List<Edge<T>>(_graphInput.Edges);

            // Формируем список смежности: для каждого узла сохраняем множество соседей.
            // Для раскраски предполагаем неориентированный граф.
            var adjacency = new Dictionary<T, HashSet<T>>(_comparer);
            foreach (var node in nodes)
            {
                adjacency[node] = new HashSet<T>(_comparer);
            }
            foreach (var edge in edges)
            {
                // Добавляем ребро в обе стороны
                if (adjacency.ContainsKey(edge.Source))
                    adjacency[edge.Source].Add(edge.Target);
                if (adjacency.ContainsKey(edge.Target))
                    adjacency[edge.Target].Add(edge.Source);
            }

            // Словарь для хранения назначения цвета для каждой вершины
            var colorAssignment = new Dictionary<T, int>(_comparer);

            // Опционально: упорядочим вершины по убыванию степени для лучшей раскраски
            var orderedNodes = nodes.OrderByDescending(n => adjacency[n].Count).ToList();

            // Для каждой вершины назначаем минимальный доступный цвет, не используемый соседями
            foreach (var vertex in orderedNodes)
            {
                // Собираем цвета, уже назначенные соседям
                var neighborColors = new HashSet<int>();
                foreach (var neighbor in adjacency[vertex])
                {
                    if (colorAssignment.TryGetValue(neighbor, out int color))
                        neighborColors.Add(color);
                }
                // Находим минимальное неотрицательное число, которого нет среди neighborColors
                int assignedColor = 0;
                while (neighborColors.Contains(assignedColor))
                {
                    assignedColor++;
                }
                colorAssignment[vertex] = assignedColor;
            }

            // Формируем список ColoredVertex из назначения
            var solution = colorAssignment.Select(kvp => new ColoredVertex<T>(kvp.Key, kvp.Value))
                                          .ToList();

            // Количество использованных цветов = максимальный номер + 1
            int numColors = colorAssignment.Values.Any() ? colorAssignment.Values.Max() + 1 : 0;

            return new SolutionResponse<ColoredVertex<T>>(solution, numColors);
        }
    }
}
