using AS_2025.Common;
using AS_2025.Domain.Common;
using AS_2025.Import.Handlers;
using NPOI.OpenXmlFormats.Dml;

namespace AS_2025.Import;

public class RelationshipBuilder : BaseDataImportHandler
{
    public RelationshipBuilder(IContext context, ImportDataContext importDataContext) 
        : base(context, importDataContext)
    {
    }

    public async Task BuildAsync(CancellationToken cancellationToken)
    {
        foreach (var departmentsGrouping in ImportDataContext.Employees.ToLookup(x => x.DepartmentId, x => x))
        {
            if (!TryGetDepartment(departmentsGrouping.Key, out var department))
            {
                continue;
            }

            var externalIds = departmentsGrouping.ToList().Select(x => x.Id);
            department.Employees = Context.Employees.Where(x => externalIds.Contains(x.ExternalId)).ToList();
            department.Head = department.Employees.FirstOrDefault(x => x.Type == EmployeeType.DepartmentHead);
        }
        await Context.SaveChangesAsync(cancellationToken);

        foreach (var employeeImported in ImportDataContext.Employees)
        {
            if (!TryGetEmployee(employeeImported.Id, out var employee))
            {
                continue;
            }

            if (TryGetEmployee(employeeImported.ManagerId, out var manager))
            {
                employee.Manager = manager;
            }

            if (TryGetTeam(employeeImported.TeamId, out var team))
            {
                employee.Team = team;
            }
        }
        await Context.SaveChangesAsync(cancellationToken);

        foreach (var teamImported in ImportDataContext.Teams)
        {
            if (!TryGetTeam(teamImported.Id, out var team))
            {
                continue;
            }

            if (TryGetDepartment(teamImported.DepartmentId, out var department))
            {
                team.Department = department;
            }

            if (TryGetEmployee(teamImported.TeamLeadId, out var teamLead))
            {
                team.TeamLead = teamLead;
            }
        }
        await Context.SaveChangesAsync(cancellationToken);
    }
}