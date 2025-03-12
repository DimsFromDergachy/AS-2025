using AS_2025.Domain.Common;
using AS_2025.Domain.Entities;
using AS_2025.Schema.List;

namespace AS_2025.Api.Employee.List;

public record ListEmployeesItem
{
    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Title = "Name", Order = 0)]
    public string FullName { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Title = "Type", Order = 1)]
    public string Type { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Tag, Title = "Level", Order = 2, TagReferenceEnum = nameof(EmployeeLevel))]
    public string Level { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Title = "Hire Date", Order = 3)]
    public DateOnly HireDate { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Integer, Title = "Salary", Order = 4, DisplayPattern = "{salary}$")]
    public decimal Salary { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public List<EmployeeSkill> Skills { get; init; } = new();

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Title = "Manager", Order = 5)]
    public string Manager { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string TeamId { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Link, Title = "Team", Order = 6, UrlPattern = "/team/{teamId}")]
    public string TeamName { get; init; }
}