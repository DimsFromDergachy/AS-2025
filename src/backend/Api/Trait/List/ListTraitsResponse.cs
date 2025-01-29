namespace AS_2025.Api.Trait.List;

public record ListTraitsResponse
{
    public IReadOnlyCollection<ListTraitsItem> Items { get; init; } = Array.Empty<ListTraitsItem>();
}