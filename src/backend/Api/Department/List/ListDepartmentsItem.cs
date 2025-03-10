namespace AS_2025.Api.Department.List;

public record ListDepartmentsItem
{
    public Guid Id { get; init; }

    public string Url { get; init; }

    public string Name { get; init; }

    public string Head { get; init; }

    public int TeamsCount { get; init; }

    public int EmployeesCount { get; init; }
}