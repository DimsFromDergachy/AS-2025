using AS_2025.Domain.Common;
using AS_2025.ReferenceItem;
using AS_2025.Schema.Form;

namespace AS_2025.Api.TableControlsPresentation.Create;

[FormSchema(Title = "Demo form")]
public record CreateTableControlsPresentationsItem
{
    [FormInputSchema(VisibilityType = FormInputVisibilityType.Hidden, DisplayType = FormInputDisplayType.None)]
    public Guid Id { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.String, Order = 1, Label = "String", Placeholder = "Put string here")]
    public string @String { get; init; }

    [FormInputSchema(VisibilityType = FormInputVisibilityType.Hidden, DisplayType = FormInputDisplayType.None)]
    public string StringPattern { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Text, Order = 2, Label = "Text", Placeholder = "Put text here")]
    public string Text { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Integer, Order = 3, Label = "Integer", Placeholder = "Put integer here", NumberFormatUseGrouping = true)]
    public int Integer { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Decimal, Order = 4, Label = "Decimal", Placeholder = "Put decimal here", NumberFormatMaximumFractionDigits = 2)]
    public double Decimal { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Date, Order = 5, Label = "Date")]
    public DateOnly Date { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Checkbox, Order = 6, Label = "Checkbox")]
    public bool Checkbox { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Percent, Order = 7, Label = "Percent")]
    public int Percent { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.References, Order = 8, Label = "References to enum", ReferenceType = ReferenceType.Enum, ReferenceName = nameof(EmployeeLevel))]
    public List<EmployeeLevel> References { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Reference, Order = 9, Label = "Reference to enum", ReferenceType = ReferenceType.Enum, ReferenceName = nameof(TaskPriority))]
    public List<TaskPriority> Reference { get; init; }

    [FormInputSchema(DisplayType = FormInputDisplayType.Reference, Order = 10, Label = "Reference to Developers", ReferenceType = ReferenceType.Model, ReferenceName = nameof(Employee), ReferenceRequest = "{\"Filter\":{\"Type\":\"Developer\"}}")]
    public Guid Developers { get; init; }
}
