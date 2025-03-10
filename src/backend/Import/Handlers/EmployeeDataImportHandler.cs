using AS_2025.Common;
using AS_2025.Domain.Common;
using AS_2025.Import.Filesystem.Model;

namespace AS_2025.Import.Handlers;

public class EmployeeDataImportHandler : BaseDataImportHandler, IDataImportHandler<Employee>
{
    public EmployeeDataImportHandler(IContext context) : base(context)
    {
    }

    public async Task HandleAsync(List<Employee> data, CancellationToken cancellationToken)
    {
        var employeesImported = data.Select(x => new Domain.Entities.Employee()
        {
            ExternalId = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Type = Enum.TryParse(typeof(EmployeeType), x.Type, out var type) ? (EmployeeType) type : EmployeeType.Undefined,
            Level = Enum.TryParse(typeof(EmployeeLevel), x.Level, out var level) ? (EmployeeLevel) level : EmployeeLevel.Undefined,
            HireDate = DateOnly.TryParse(x.HireDate, out var hireDate) ? hireDate : DateOnly.FromDateTime(DateTime.Now),
            Salary = decimal.TryParse(x.Salary, out var salary) ? salary : 0,
            Manager = TryGetEmployee(x.ManagerId, out var manager) ? manager : null,
            Team = TryGetTeam(x.TeamId, out var team) ? team : null,
        }).ToList();

        foreach (var employee in employeesImported)
        {
            if (!TryGetEmployee(employee.ExternalId, out var existing))
            {
                Context.Employees.Add(employee);
                continue;
            }

            existing.Update(employee);
        }

        foreach (var departmentsGrouping in employeesImported.ToLookup(x => data.First(d => d.Id == x.ExternalId).DepartmentId, x => x))
        {
            if (!TryGetDepartment(departmentsGrouping.Key, out var department))
            {
                continue;
            }

            var externalIds = departmentsGrouping.ToList().Select(x => x.ExternalId);
            department.Employees = Context.Employees.Where(x => externalIds.Contains(x.ExternalId)).ToList();
            department.Head = department.Employees.FirstOrDefault(x => x.Type == EmployeeType.DepartmentHead);
        }

        await Context.SaveChangesAsync(cancellationToken);
    }
}