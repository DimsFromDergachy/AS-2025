using AS_2025.ReferenceItem;
using AS_2025.Tags;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
[TaggedEnum]
public enum TaskPriority
{
    [TagStyle(TagStyle.Blue)]
    Low,
    [TagStyle(TagStyle.Green)]
    Medium,
    [TagStyle(TagStyle.Gold)]
    High,
    [TagStyle(TagStyle.Red)]
    Critical
}