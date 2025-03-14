using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
public enum ClientStatus
{
    [ReferenceIgnore]
    Undefined,
    Lead,
    Prospect,
    Active,
    Inactive,
    Former
}