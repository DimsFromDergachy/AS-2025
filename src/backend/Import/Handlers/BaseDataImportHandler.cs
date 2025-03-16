using System.Diagnostics.CodeAnalysis;
using AS_2025.Common;

namespace AS_2025.Import.Handlers;

public abstract class BaseDataImportHandler
{
    internal IContext Context;
    internal ImportDataContext ImportDataContext;

    protected BaseDataImportHandler(IContext context, ImportDataContext importDataContext)
    {
        Context = context;
        ImportDataContext = importDataContext;
    }

    protected bool TryGetDepartment(string externalId, [NotNullWhen(true)] out Domain.Entities.Department? department)
    {
        department = Context.Departments.SingleOrDefault(x => x.ExternalId == externalId);
        return department is not null;
    }

    protected bool TryGetTeam(string externalId, [NotNullWhen(true)] out Domain.Entities.Team? team)
    {
        team = Context.Teams.SingleOrDefault(x => x.ExternalId == externalId);
        return team is not null;
    }

    protected bool TryGetEmployee(string externalId, [NotNullWhen(true)] out Domain.Entities.Employee? employee)
    {
        employee = Context.Employees.SingleOrDefault(x => x.ExternalId == externalId);
        return employee is not null;
    }
}