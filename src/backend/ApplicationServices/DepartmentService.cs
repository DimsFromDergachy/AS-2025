using AS_2025.Api.Department.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.ApplicationServices;

public class DepartmentService
{
    private readonly IContext _context;

    public DepartmentService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Department>> ListAsync(ListDepartmentsFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteDepartmentRequest request, CancellationToken cancellationToken)
    {
        return _context.Departments
            .Include(x => x.Head)
            .Include(x => x.Teams)
            .Include(x => x.Employees)
            .Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
