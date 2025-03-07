using System.ComponentModel.DataAnnotations;
using AS_2025.Domain.Common;

namespace AS_2025.Domain.Entities;

public record Department : Entity<Guid>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    [Required] 
    public string Name { get; set; } = string.Empty;

    public Employee Head { get; set; }

    public List<Employee> Employees { get; set; } = new();

    public List<Team> Teams { get; set; } = new();
}