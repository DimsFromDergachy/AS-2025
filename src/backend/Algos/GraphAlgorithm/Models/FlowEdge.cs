namespace AS_2025.Algos.GraphAlgorithm.Models
{
    public record FlowEdge<T>(T Source, T Sink, double Flow, double Capacity);
}
