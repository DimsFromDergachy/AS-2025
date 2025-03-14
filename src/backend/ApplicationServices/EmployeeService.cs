using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.ApplicationServices;

public class EmployeeService
{
    private readonly IContext _context;

    public EmployeeService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Employee>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Include(x => x.Manager)
            .Include(x => x.Team)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}