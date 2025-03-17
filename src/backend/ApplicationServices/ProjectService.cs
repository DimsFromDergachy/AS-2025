using AS_2025.Api.Project.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.ApplicationServices;

public class ProjectService
{
    private readonly IContext _context;

    public ProjectService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Project>> ListAsync(ListProjectsFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        return _context.Projects.Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
