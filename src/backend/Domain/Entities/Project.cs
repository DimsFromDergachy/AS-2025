using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AS_2025.Domain.Entities;

public record Project : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; }

    public string ExternalId { get; init; } = string.Empty;

    [Required] 
    public string Name { get; set; } = string.Empty;

    [Required] 
    public string Description { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ProjectStatus Status { get; set; } = ProjectStatus.Planning;

    [Required]
    public decimal Budget { get; set; }

    public Client Client { get; set; }

    public Employee? ProjectManager { get; set; }

    public List<Team> AssignedTeams { get; set; } = new();

    public List<Employee> AdditionalMembers { get; set; } = new();

    public List<Task> Tasks { get; set; } = new();
}