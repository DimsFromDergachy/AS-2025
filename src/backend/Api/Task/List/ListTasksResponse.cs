namespace AS_2025.Api.Task.List;

public record ListTasksResponse
{
    public IReadOnlyCollection<ListTasksItem> Items { get; init; } = Array.Empty<ListTasksItem>();
}
