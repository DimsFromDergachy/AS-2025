using AS_2025.Api.Employee.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.ApplicationServices;

public class EmployeeService
{
    private readonly IContext _context;

    public EmployeeService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Employee>> ListAsync(ListEmployeesFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Include(x => x.Manager)
            .Include(x => x.Team)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        return _context.Employees
            .Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
