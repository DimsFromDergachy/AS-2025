using AS_2025.Common;
using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
public enum ClientType
{
    [ReferenceIgnore]
    Undefined,
    Enterprise,
    SMB,
    Startup,
    Government,
    [StringValue("Non-Profit")]
    NonProfit,
    Individual
}