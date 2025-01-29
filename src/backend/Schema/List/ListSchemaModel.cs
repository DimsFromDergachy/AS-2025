namespace AS_2025.Schema.List;

public enum FieldType
{
    String
}

public enum FieldDisplayType
{
    None,
    String,
    Link
}

public record ListSchemaModel
{
    public IReadOnlyList<ListSchemaColumnModel> Columns { get; init; } = new List<ListSchemaColumnModel>();
}

public record ListSchemaColumnModel
{
    public string Key { get; init; } = string.Empty;

    public int Order { get; init; } = -1;

    public bool IsVisible { get; init; } = false;

    public bool IsVisibleByDefault { get; init; } = false;

    public FieldType FieldType { get; init; } = FieldType.String;

    public FieldDisplayType FieldDisplayType { get; init; } = FieldDisplayType.None;
}