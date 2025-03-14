using AS_2025.Schema.List;

namespace AS_2025.Api.Client.List;

public record ListClientsItem
{
    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }
}
