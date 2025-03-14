using AS_2025.Common;
using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
public enum TeamType
{
    [ReferenceIgnore]
    Undefined,
    Development,
    QA,
    Design,
    DevOps,
    FullStack,
    Frontend,
    Backend,
    Mobile,
    [StringValue("Cross Functional")]
    CrossFunctional
}