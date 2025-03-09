using AS_2025.Domain.Common;

namespace AS_2025.Domain.Entities;

public record Team : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public TeamType Type { get; set; } = TeamType.Undefined;

    public Department? Department { get; set; }

    public Employee? TeamLead { get; set; }

    public List<Employee> Members { get; set; } = new();

    public List<Project> AssignedProjects { get; set; } = new();
}