using System.ComponentModel.DataAnnotations;
using AS_2025.Domain.Common;

namespace AS_2025.Domain.Entities;

public record Employee : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public EmployeeType Type { get; set; } = EmployeeType.Undefined;

    public EmployeeLevel Level { get; set; } = EmployeeLevel.Undefined;

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    public decimal Salary { get; set; }

    public List<EmployeeSkill> Skills { get; set; } = new();
    
    public Employee? Manager { get; init; }

    public Team? Team { get; init; }
    
    public string FullName => $"{FirstName} {LastName}";
}