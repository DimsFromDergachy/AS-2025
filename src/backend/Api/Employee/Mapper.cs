using AS_2025.Api.Employee.List;
using AS_2025.Common;
using AS_2025.Domain.Common;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Employee;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Department.Id), nameof(ListEmployeesItem.Url), Use = nameof(GuidToUrl))]
    [MapProperty("Manager.FullName", "Manager")]
    [MapProperty("Team.Id", "TeamId")]
    [MapProperty("Team.Name", "TeamName")]
    [MapProperty("Type", "Type", Use = nameof(EmployeeTypeToString))]
    [MapProperty("Level", "Level", Use = nameof(EmployeeLevelToString))]
    [MapValue("Efficiency", Use = nameof(GetEfficiency))]
    public static partial ListEmployeesItem ToListItem(Domain.Entities.Employee entity);

    [UserMapping(Default = false)]
    private static string GuidToUrl(Guid id)
    {
        return $"/employee/{id}";
    }

    private static string EmployeeTypeToString(EmployeeType employeeType)
    {
        return employeeType.GetStringValue();
    }

    private static string EmployeeLevelToString(EmployeeLevel employeeLevel)
    {
        return employeeLevel.GetStringValue();
    }

    private static int GetEfficiency()
    {
        return new Random().Next(0, 100);
    }
}