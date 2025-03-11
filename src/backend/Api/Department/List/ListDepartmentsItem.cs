using AS_2025.Schema.List;

namespace AS_2025.Api.Department.List;

public record ListDepartmentsItem
{
    [ListColumnSchema(DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }

    [ListColumnSchema(Order = 1, DisplayType = ListColumnDisplayType.Link, UrlPattern = "/api/department/{id}")]
    public string Name { get; init; }

    [ListColumnSchema(Order = 2, DisplayType = ListColumnDisplayType.String)]
    public string Head { get; init; }

    [ListColumnSchema(Order = 3, DisplayType = ListColumnDisplayType.Integer)]
    public int TeamsCount { get; init; }

    [ListColumnSchema(Order = 4, DisplayType = ListColumnDisplayType.Integer)]
    public int EmployeesCount { get; init; }
}