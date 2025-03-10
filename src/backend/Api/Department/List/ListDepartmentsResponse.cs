namespace AS_2025.Api.Department.List;

public record ListDepartmentsResponse
{
    public IReadOnlyCollection<ListDepartmentsItem> Items { get; init; } = Array.Empty<ListDepartmentsItem>();
}