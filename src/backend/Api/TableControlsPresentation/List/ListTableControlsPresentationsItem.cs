using AS_2025.Domain.Common;
using AS_2025.Schema.List;

namespace AS_2025.Api.TableControlsPresentation.List;

public record ListTableControlsPresentationsItem
{
    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Order = 1, Title = "String")]
    public string @String { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Order = 2, Title = "StringPattern", DisplayPattern = "String is `{stringPattern}`")]
    public string StringPattern { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Text, Order = 3, Title = "Text")]
    public string Text { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Integer, Order = 4, Title = "Integer")]
    public int Integer { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Decimal, Order = 5, Title = "Decimal", NumberFormatUseGrouping = true, NumberFormatMaximumFractionDigits = 2)]
    public double Decimal { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Date, Order = 6, Title = "Date")]
    public DateOnly Date { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Checkbox, Order = 7, Title = "Checkbox")]
    public bool Checkbox { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Link, Order = 8, Title = "Link", UrlPattern = "{link}/suffix")]
    public string Link { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Percent, Order = 8, Title = "Percent")]
    public int Percent { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Tags, Order = 10, Title = "Tags", TagReferenceEnum = nameof(EmployeeLevel))]
    public List<EmployeeLevel> Tags { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Tags, Order = 11, Title = "ComplexTags", TagReferenceEnum = nameof(EmployeeLevel), TagField = "tag")]
    public List<ComplexTag> ComplexTags { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Tag, Order = 12, Title = "Tag", TagReferenceEnum = nameof(TaskPriority))]
    public TaskPriority Tag { get; init; }
}

public record ComplexTag(EmployeeLevel Tag, int Level);