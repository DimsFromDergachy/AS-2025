using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.ApplicationServices;

public class ProjectService
{
    private readonly IContext _context;

    public ProjectService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Project>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
