using AS_2025.Api.Team.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.ApplicationServices;

public class TeamService
{
    private readonly IContext _context;

    public TeamService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Team>> ListAsync(ListTeamsFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Teams
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        return _context.Teams
            .Include(x => x.Department)
            .Include(x => x.TeamLead)
            .Include(x => x.Members)
            .Include(x => x.AssignedProjects)
            .Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
