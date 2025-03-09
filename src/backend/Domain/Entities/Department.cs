using System.ComponentModel.DataAnnotations;
using AS_2025.Domain.Common;

namespace AS_2025.Domain.Entities;

public record Department : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;

    [Required] 
    public string Name { get; set; } = string.Empty;

    [Required]
    public Employee Head { get; set; }

    public List<Team> Teams { get; set; } = new();

    public List<Employee> Employees { get; set; } = new();
}