using AS_2025.Common;
using AS_2025.ReferenceItem;

namespace AS_2025.Domain.Common;

[ReferenceEnum]
public enum EmployeeType
{
    [ReferenceIgnore]
    Undefined,
    Developer,
    [StringValue("QA Engineer")]
    QAEngineer,
    [StringValue("Project Manager")]
    ProjectManager,
    [StringValue("Business Analyst")]
    BusinessAnalyst,
    [StringValue("UX Designer")]
    UXDesigner,
    [StringValue("DevOps Engineer")]
    DevOpsEngineer,
    [StringValue("Support Engineer")]
    SupportEngineer,
    [StringValue("Team Lead")]
    TeamLead,
    [StringValue("Department Head")]
    DepartmentHead,
    CEO
}