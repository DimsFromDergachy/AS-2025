using System.ComponentModel.DataAnnotations;
using AS_2025.Domain.Common;
using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Entities;

public record Department : Entity<Guid>, IIdentifiableEntity<string>
{
    [ReferenceKey]
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;

    [Required]
    [ReferenceValue]
    public string Name { get; set; } = string.Empty;

    public Employee? Head { get; set; }

    public List<Team> Teams { get; set; } = new();

    public List<Employee> Employees { get; set; } = new();

    public Department Update(Department other)
    {
        Name = other.Name;

        return this;
    }

    public static Department Create()
    {
        var guid = Guid.CreateVersion7();
        return new Department()
        {
            Id = guid,
            ExternalId = guid.ToString()
        };
    }
}