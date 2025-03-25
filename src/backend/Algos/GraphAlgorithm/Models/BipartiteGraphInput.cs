namespace AS_2025.Algos.GraphAlgorithm.Models
{
    public record BipartiteGraphInput<T>(IEnumerable<T> Left, IEnumerable<T> Right, IEnumerable<Edge<T>> Edges);
}
