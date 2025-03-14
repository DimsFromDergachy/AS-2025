using AS_2025.Common;
using Microsoft.EntityFrameworkCore;
using Task = AS_2025.Domain.Entities.Task;

namespace AS_2025.ApplicationServices;

public class TaskService
{
    private readonly IContext _context;

    public TaskService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Task>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
