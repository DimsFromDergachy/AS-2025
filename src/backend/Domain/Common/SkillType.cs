using AS_2025.Common;
using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
public enum SkillType
{
    [ReferenceIgnore]
    Undefined,
    [StringValue("C#")]
    CSharp,
    Java,
    JavaScript
}