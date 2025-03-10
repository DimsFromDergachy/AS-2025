using AS_2025.Domain.Entities;

namespace AS_2025.Api.Employee.List;

public record ListEmployeesItem
{
    public Guid Id { get; init; }

    public string Url { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Type { get; init; }

    public string Level { get; init; }

    public DateOnly HireDate { get; init; }

    public decimal Salary { get; init; }

    public List<EmployeeSkill> Skills { get; init; } = new();

    public string Manager { get; init; }

    public string TeamId{ get; init; }

    public string TeamName { get; init; }
}