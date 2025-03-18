using AS_2025.ReferenceItem;

namespace AS_2025.Schema.Form;

public record FormInputSchemaItem
{
    public string Key { get; init; }

    public string Label { get; init; }

    public string Placeholder { get; init; }

    public int Order { get; init; }

    public FormInputVisibilityType VisibilityType { get; init; }

    public FormInputDisplayType DisplayType { get; init; }

    public ReferenceType ReferenceType { get; init; }

    public string? ReferenceName { get; init; }

    public string? ReferenceRequest { get; init; }

    public NumberFormat? NumberFormat { get; init; }

    public static FormInputSchemaItem From(FormInputSchemaAttribute attribute, string key)
    {
        return new FormInputSchemaItem
        {
            Key = key,
            Label = attribute.Label,
            Placeholder = attribute.Placeholder,
            Order = attribute.Order,
            VisibilityType = attribute.VisibilityType,
            DisplayType = attribute.DisplayType,
            ReferenceType = attribute.ReferenceType,
            ReferenceName = attribute.ReferenceName,
            ReferenceRequest = attribute.ReferenceRequest,
            NumberFormat = NumberFormat.From(attribute)
        };
    }
}