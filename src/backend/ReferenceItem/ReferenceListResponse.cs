namespace AS_2025.ReferenceItem;

public record ReferenceListResponse
{
    public IReadOnlyCollection<ReferenceItem> Items { get; init; } = new List<ReferenceItem>();
}