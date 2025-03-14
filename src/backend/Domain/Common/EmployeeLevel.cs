using AS_2025.ReferenceItem;
using AS_2025.Tags;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
[TaggedEnum]
public enum EmployeeLevel
{
    [ReferenceIgnore]
    Undefined,
    [TagStyle(TagStyle.Lime)]
    Junior,
    [TagStyle(TagStyle.Geekblue)]
    Middle,
    [TagStyle(TagStyle.Volcano)]
    Senior,
    [TagStyle(TagStyle.Purple)]
    Lead
}