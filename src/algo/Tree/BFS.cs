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
        visited[start] = true;

        var queue = new Queue<TVertex>();
        queue.Enqueue(start);

        while (queue.Any())
        {
            var current = queue.Dequeue();

            yield return current;

            foreach (var e in edges)
            {
                if (e.a.Equals(current) && !visited[e.b])
                {
                    visited[e.b] = true;
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
        var edges = new[] {('A', 'A'), ('A', 'B'), ('E', 'B'), ('B', 'D'),
                           ('D', 'B'), ('A', 'E'), ('E', 'D'), ('Z', 'X')};
        var graph = (vs, edges);

        Assert.Equal(graph.Path('A'), ['A', 'B', 'E', 'D']);
        Assert.Equal(graph.Path('B'), ['B', 'D']);
        Assert.Equal(graph.Path('D'), ['D', 'B']);
        Assert.Equal(graph.Path('E'), ['E', 'B', 'D']);
        Assert.Equal(graph.Path('X'), ['X']);
        Assert.Equal(graph.Path('Z'), ['Z', 'X']);

        Assert.True(graph.Connected('A', 'A'));
        Assert.True(graph.Connected('A', 'B'));
        Assert.True(graph.Connected('A', 'D'));
        Assert.True(graph.Connected('A', 'E'));
        Assert.False(graph.Connected('A', 'X'));
        Assert.False(graph.Connected('A', 'Z'));
        Assert.False(graph.Connected('A', 'R'));

        Assert.True(graph.Connected('Z', 'X'));
        Assert.False(graph.Connected('X', 'Z'));
    }
}
