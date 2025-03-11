namespace AS_2025.Schema.List;

public record ListSchemaModel
{
    public IReadOnlyList<ListColumnSchemaItem> Columns { get; init; } = new List<ListColumnSchemaItem>();
}