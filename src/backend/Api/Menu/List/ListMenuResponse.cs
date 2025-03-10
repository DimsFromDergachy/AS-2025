using AS_2025.Api.Department.List;

namespace AS_2025.Api.Menu.List;

public record ListMenuResponse
{
    public IReadOnlyCollection<ListMenuItem> Items { get; init; } = Array.Empty<ListMenuItem>();
}