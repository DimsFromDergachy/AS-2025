using AS_2025.Common;
using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
public enum ProjectStatus
{
    [ReferenceIgnore]
    Undefined,
    Planning,
    [StringValue("In Progress")]
    InProgress,
    [StringValue("On Hold")]
    OnHold,
    Completed,
    Cancelled
}