namespace AS_2025.Api.Employee.List;

public record ListEmployeesResponse
{
    public IReadOnlyCollection<ListEmployeesItem> Items { get; init; } = Array.Empty<ListEmployeesItem>();
}
