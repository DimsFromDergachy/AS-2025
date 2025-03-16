using AS_2025.Import.Filesystem.Model;

namespace AS_2025.Import;

public class ImportDataContext
{
    public IReadOnlyCollection<Department> Departments { get; private set; } = new List<Department>();

    public IReadOnlyCollection<Employee> Employees { get; private set; } = new List<Employee>();

    public IReadOnlyCollection<Team> Teams { get; private set; } = new List<Team>();

    public ImportDataContext WithDepartments(IEnumerable<Department> departments)
    {
        Departments = departments.ToList();
        return this;
    }

    public ImportDataContext WithEmployees(IEnumerable<Employee> employees)
    {
        Employees = employees.ToList();
        return this;
    }

    public ImportDataContext WithTeams(IEnumerable<Team> teams)
    {
        Teams = teams.ToList();
        return this;
    }
}