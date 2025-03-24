using AS_2025.Api.Department.Create;
using AS_2025.Api.Department.Get;
using AS_2025.Api.Department.List;
using AS_2025.Api.Department.Update;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.Department;

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false, UseReferenceHandling = true)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.Department.Id), nameof(ListDepartmentsItem.Url), Use = nameof(GuidToUrl))]
    [MapProperty("Head.FullName", "Head")]
    public static partial CreateDepartmentResponse ToCreateResponse(Domain.Entities.Department entity);

    public static partial GetDepartmentResponse ToGetResponse(Domain.Entities.Department entity);

    [MapProperty(nameof(Domain.Entities.Department.Id), nameof(ListDepartmentsItem.Url), Use = nameof(GuidToUrl))]
    [MapProperty("Head.FullName", "Head")]
    [MapProperty(nameof(Domain.Entities.Department.Employees), nameof(ListDepartmentsItem.EmployeesCount), Use = nameof(EmployeesToCount))]
    [MapProperty(nameof(Domain.Entities.Department.Teams), nameof(ListDepartmentsItem.TeamsCount), Use = nameof(TeamsToCount))]
    public static partial UpdateDepartmentResponse ToUpdateResponse(Domain.Entities.Department entity);

    [MapProperty(nameof(Domain.Entities.Department.Id), nameof(ListDepartmentsItem.Url), Use = nameof(GuidToUrl))]
    [MapProperty("Head.FullName", "Head")]
    [MapProperty(nameof(Domain.Entities.Department.Employees), nameof(ListDepartmentsItem.EmployeesCount), Use = nameof(EmployeesToCount))]
    [MapProperty(nameof(Domain.Entities.Department.Teams), nameof(ListDepartmentsItem.TeamsCount), Use = nameof(TeamsToCount))]
    public static partial ListDepartmentsItem ToListItem(Domain.Entities.Department entity);

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