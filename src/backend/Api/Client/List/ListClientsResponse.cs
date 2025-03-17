namespace AS_2025.Api.Client.List;

public record ListClientsResponse
{
    public IReadOnlyCollection<ListClientsItem> Items { get; init; } = Array.Empty<ListClientsItem>();
}
