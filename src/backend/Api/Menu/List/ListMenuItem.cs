namespace AS_2025.Api.Menu.List;

public record ListMenuItem
{
    public string Link { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;
}