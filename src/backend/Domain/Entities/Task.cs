using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;
using TaskStatus = AS_2025.Domain.Common.TaskStatus;

namespace AS_2025.Domain.Entities;

public record Task : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskPriority Priority { get; set; } = TaskPriority.Low;

    public TaskStatus Status { get; set; } = TaskStatus.New;

    public DateTime CreatedDateTime { get; set; }

    public DateTime? CompletedDateTime { get; set; }

    public int EstimatedHours { get; set; }

    public int ActualHours { get; set; }

    public Employee? AssignedTo { get; set; }
}