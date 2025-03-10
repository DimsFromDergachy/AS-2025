using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.ApplicationServices;

public class DepartmentService
{
    private readonly IContext _context;

    public DepartmentService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Department>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Include(x => x.Head)
            .Include(x => x.Teams)
            .Include(x => x.Employees)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}