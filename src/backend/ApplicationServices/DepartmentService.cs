using AS_2025.Api.Department.Create;
using AS_2025.Api.Department.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.Domain.Common;
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

    public async Task<Department> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var department = Department.Create();
        department.Name = request.Name;

        if (request.HeadId is not null)
        {
            var head = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == request.HeadId && x.Type == EmployeeType.DepartmentHead, cancellationToken);
            department.Head = head;
        }

        await _context.Departments.AddAsync(department, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return department;
    }

    public async Task<IReadOnlyCollection<Department>> ListAsync(ListDepartmentsFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Include(x => x.Head)
            .Include(x => x.Teams)
            .Include(x => x.Employees)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteDepartmentRequest request, CancellationToken cancellationToken)
    {
        return _context.Departments
            .Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
