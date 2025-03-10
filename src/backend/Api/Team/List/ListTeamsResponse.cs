namespace AS_2025.Api.Team.List;

public record ListTeamsResponse
{
    public IReadOnlyCollection<ListTeamsItem> Items { get; init; } = Array.Empty<ListTeamsItem>();
}