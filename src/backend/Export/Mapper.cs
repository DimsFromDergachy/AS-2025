using AS_2025.Export.Model;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Export;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Department.Id), nameof(Department.Url), Use = nameof(GuidToUrl))]
    [MapProperty("Head.FullName", "Head")]
    [MapProperty(nameof(Domain.Entities.Department.Employees), nameof(Department.EmployeesCount), Use = nameof(EmployeesToCount))]
    [MapProperty(nameof(Domain.Entities.Department.Teams), nameof(Department.TeamsCount), Use = nameof(TeamsToCount))]
    public static partial Department ToExportModel(Domain.Entities.Department entity);

    private static string GuidToUrl(Guid id)
    {
        return $"/department/{id}";
    }

    private static int EmployeesToCount(List<Domain.Entities.Employee> employees)
    {
        return employees.Count;
    }

    private static int TeamsToCount(List<Domain.Entities.Team> teams)
    {
        return teams.Count;
    }
}