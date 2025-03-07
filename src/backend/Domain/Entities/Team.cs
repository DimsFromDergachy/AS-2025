using AS_2025.Domain.Common;

namespace AS_2025.Domain.Entities;

public record Team : Entity<Guid>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string Name { get; set; } = string.Empty;

    public Employee? TeamLead { get; set; }

    public List<Employee> Members { get; set; } = new();

    public TeamType Type { get; set; }

    public Department Department { get; set; }

    public List<Project> AssignedProjects { get; set; } = new();
}