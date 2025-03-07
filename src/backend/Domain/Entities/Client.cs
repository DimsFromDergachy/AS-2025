using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AS_2025.Domain.Entities;

public record Client : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string Identity { get; init; } = string.Empty;

    [Required]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public ClientType Type { get; set; }

    public List<Project> Projects { get; set; } = new();

    public List<ContactPerson> Contacts { get; set; } = new();

    [Required]
    public DateTime PartnershipDateTime { get; set; }

    public ClientStatus Status { get; set; } = ClientStatus.Unknown;

    public Employee? AccountManager { get; set; }
}