namespace AS_2025.Algos.DijkstraAlgorithm.Models
{
    public record GraphInput<T>(IEnumerable<T> Nodes, IEnumerable<Edge<T>> Edges, T Start, T Target);
}
