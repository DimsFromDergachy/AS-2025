namespace AS_2025.Schema.List;

// TODO think about inheritance
public class ListColumnSchemaItem
{
    public string Key { get; init; }

    public string Title { get; init; }

    public int Order { get; init; }

    public ListColumnVisibilityType VisibilityType { get; init; }

    public ListColumnDisplayType DisplayType { get; init; }

    public string DisplayPattern { get; init; }

    public string UrlPattern { get; init; }

    public bool Filterable { get; init; }

    public bool Sortable { get; init; }

    public static ListColumnSchemaItem From(ListColumnSchemaAttribute attribute, string key)
    {
        return new ListColumnSchemaItem
        {
            Key = key,
            Title = attribute.Title,
            Order = attribute.Order,
            VisibilityType = attribute.VisibilityType,
            DisplayType = attribute.DisplayType,
            DisplayPattern = attribute.DisplayPattern,
            UrlPattern = attribute.UrlPattern,
            Filterable = attribute.Filterable,
            Sortable = attribute.Sortable
        };
    }
}