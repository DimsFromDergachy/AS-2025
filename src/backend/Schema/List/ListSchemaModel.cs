namespace AS_2025.Schema.List;

public record ListSchemaModel
{
    public string Title { get; init; } = string.Empty;

    public ListColumnSchemaItem[] Columns { get; init; } = Array.Empty<ListColumnSchemaItem>();

    public ListAction[] ColumnActions { get; init; } = new[] { ListAction.View, ListAction.Edit, ListAction.Delete };

    public ListAction[] CommonActions { get; init; } = new[] { ListAction.Search, ListAction.Create };
}