using AS_2025.Api.Task.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.ApplicationServices;

public class TaskService
{
    private readonly IContext _context;

    public TaskService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<AS_2025.Domain.Entities.Task>> ListAsync(ListTasksFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        return _context.Tasks.Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
