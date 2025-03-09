using AS_2025.Common;
using AS_2025.Import.Filesystem.Model;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Import.Handlers;

public class DepartmentDataImportHandler : IDataImportHandler<Department>
{
    private readonly IContext _context;

    public DepartmentDataImportHandler(IContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(IEnumerable<Department> data, CancellationToken cancellationToken)
    {
        var departmentsImported = data.Select(x => new Domain.Entities.Department()
        {
            ExternalId = x.Id,
            Name = x.Name
        });

        foreach (var department in departmentsImported)
        {
            await _context.Departments.Upsert(department)
                .AllowIdentityMatch()
                .On(x => x.ExternalId)
                .WhenMatched((existing, imported) => new Domain.Entities.Department()
                {
                    Id = existing.Id,
                    ExternalId = existing.ExternalId,
                    Name = imported.Name
                })
                .RunAsync(cancellationToken);
        }
    }
}