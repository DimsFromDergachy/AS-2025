using AS_2025.Schema.List;

namespace AS_2025.Api.Department.List;

[ListSchema(Title = "Departments", ColumnActions = new [] { ListAction.View, ListAction.Edit, ListAction.Delete }, CommonActions = new [] { ListAction.Search, ListAction.Create, ListAction.ExportXlsx })]
public record ListDepartmentsItem
{
    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Link, Title = "Name", Order = 1, UrlPattern = "/api/department/{id}")]
    public string Name { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.String, Title = "Head", Order = 2)]
    public string Head { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Integer, Title = "Teams Count", Order = 3)]
    public int TeamsCount { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.Integer, Title = "Employees Count", Order = 4)]
    public int EmployeesCount { get; init; }
}