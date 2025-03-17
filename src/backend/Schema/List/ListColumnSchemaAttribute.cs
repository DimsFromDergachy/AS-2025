namespace AS_2025.Schema.List;

[AttributeUsage(AttributeTargets.Property)]
public class ListColumnSchemaAttribute : Attribute
{
    public string Title { get; init; }

    public int Order { get; init; }

    public ListColumnVisibilityType VisibilityType { get; init; } = ListColumnVisibilityType.Visible;

    public ListColumnDisplayType DisplayType { get; init; } = ListColumnDisplayType.String;

    public string DisplayPattern { get; init; }

    public string UrlPattern { get; init; }

    public bool Filterable { get; init; }

    public bool Sortable { get; init; }

    public bool Searchable { get; init; }

    public string TagReferenceEnum { get; init; }

    public string TagField { get; init; }

    public bool NumberFormatUseGrouping { get; init; } = true;

    public int NumberFormatMaximumFractionDigits { get; init; } = 20;
}