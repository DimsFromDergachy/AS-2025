namespace AS_2025.Schema.List;

[AttributeUsage(AttributeTargets.Class)]
public class ListSchemaAttribute : Attribute
{
    public string Title { get; init; } = string.Empty;

    public ListAction[] ColumnActions { get; init; } = new[] { ListAction.None };

    public ListAction[] CommonActions { get; init; } = new[] { ListAction.None };
}