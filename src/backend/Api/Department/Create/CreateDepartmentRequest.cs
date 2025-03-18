using Ardalis.Result;
using AS_2025.ReferenceItem;
using AS_2025.Schema.Form;
using MediatR;

namespace AS_2025.Api.Department.Create;

[FormSchema(Title = "Department")]
public record CreateDepartmentRequest : IRequest<Result<CreateDepartmentResponse>>
{
    [FormInputSchema(DisplayType = FormInputDisplayType.String, Order = 1, Label = "Name", Placeholder = "Department name")]
    public string Name { get; set; } = string.Empty;

    [FormInputSchema(DisplayType = FormInputDisplayType.Reference, Order = 2, Label = "Head", ReferenceType = ReferenceType.Model, ReferenceName = "employee", ReferenceRequest = "{\"Filter\": {\"Type\": \"DepartmentHead\"}}")]
    public Guid? HeadId { get; set; }
}