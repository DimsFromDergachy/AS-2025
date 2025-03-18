using AS_2025.ReferenceItem;

namespace AS_2025.Schema.Form;

[AttributeUsage(AttributeTargets.Property)]
public class FormInputSchemaAttribute : Attribute
{
    public string Label { get; init; }

    public string Placeholder { get; init; }

    public int Order { get; init; }

    public FormInputVisibilityType VisibilityType { get; init; }

    public FormInputDisplayType DisplayType { get; init; }

    public ReferenceType ReferenceType { get; init; }

    public string? ReferenceName { get; init; }

    public string? ReferenceRequest { get; init; }

    public bool NumberFormatUseGrouping { get; init; } = true;

    public int NumberFormatMaximumFractionDigits { get; init; } = 20;
}