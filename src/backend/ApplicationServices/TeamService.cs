using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.ApplicationServices;

public class TeamService
{
    private readonly IContext _context;

    public TeamService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Team>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Teams
            .Include(x => x.Department)
            .Include(x => x.TeamLead)
            .Include(x => x.Members)
            .Include(x => x.AssignedProjects)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
