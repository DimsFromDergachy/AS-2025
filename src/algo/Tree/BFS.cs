namespace Algo.Tree;

public static class BFS
{
    public static IEnumerable<TVertex> Path<TVertex>(
        this (ISet<TVertex>, IList<(TVertex a, TVertex b)>) graph,
        TVertex start
        ) where TVertex: notnull
    {
        (var vs, var edges) = graph;
        var visited = vs.ToDictionary(v => v, v => false);

        var queue = new Queue<TVertex>();
        queue.Enqueue(start);

        while (queue.Any())
        {
            var current = queue.Dequeue();
            visited[current] = true;

            yield return current;

            foreach (var e in edges)
            {
                if (e.a.Equals(current) && !visited[e.b])
                {
                    queue.Enqueue(e.b);
                }
            }
        }
    }

    public static bool Connected<TVertex>(
        this (ISet<TVertex>, IList<(TVertex a, TVertex b)>) graph,
        TVertex start,
        TVertex end)
        where TVertex: notnull
    {
        return Path(graph, start).Any(v => v.Equals(end));
    }
}

public class BFSTest
{
    [Fact]
    public void Simple()
    {
        var vs = new[] {'A', 'B', 'D', 'E', 'X', 'Z'}.ToHashSet();
        var edges = new[] {('A', 'A'), ('A', 'B'),
                           ('A', 'E'), ('E', 'D'), ('Z', 'X')};
        var graph = (vs, edges);

        Assert.Equal(['A', 'B', 'E', 'D'], graph.Path('A'));
        Assert.True(graph.Connected('A', 'A'));
        Assert.True(graph.Connected('A', 'B'));
        Assert.True(graph.Connected('A', 'D'));
        Assert.True(graph.Connected('A', 'E'));
        Assert.False(graph.Connected('A', 'X'));
        Assert.False(graph.Connected('A', 'Z'));
        Assert.False(graph.Connected('A', 'R'));
    }
}
