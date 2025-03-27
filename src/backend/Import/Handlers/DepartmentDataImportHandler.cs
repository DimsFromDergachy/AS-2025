using AS_2025.Common;
using AS_2025.Import.Filesystem.Model;

namespace AS_2025.Import.Handlers;

public class DepartmentDataImportHandler : BaseDataImportHandler, IDataImportHandler<Department>
{
    public DepartmentDataImportHandler(IContext context, ImportDataContext importDataContext) : base(context, importDataContext)
    {
    }

    public async Task HandleAsync(List<Department> data, CancellationToken cancellationToken)
    {
        ImportDataContext.WithDepartments(data);

        var departmentsImported = data.Select(x => new Domain.Entities.Department()
        {
            ExternalId = x.Id,
            Name = x.Name
        });

        foreach (var department in departmentsImported)
        {
            if (!TryGetDepartment(department.ExternalId, out var existing))
            {
                Context.Departments.Add(department);
                continue;
            }

            existing.Update(department);
        }

        await Context.SaveChangesAsync(cancellationToken);
    }
}