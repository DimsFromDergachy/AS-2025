using AS_2025.Menu;

namespace AS_2025.Api.Menu.List;

public record ListMenuResponse
{
    public IReadOnlyCollection<MenuItem> Items { get; init; } = Array.Empty<MenuItem>();
}