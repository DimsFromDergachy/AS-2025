using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AS_2025.Domain.Entities;

public record Client : Entity<Guid>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    [Required]
    public string CompanyName { get; set; } = string.Empty;

    public ClientType Type { get; set; }

    public List<Project> Projects { get; set; } = new();

    public List<ContactPerson> Contacts { get; set; } = new();

    public DateTime FirstContactDate { get; set; }

    public ClientStatus Status { get; set; }

    public Employee? AccountManager { get; set; }
}