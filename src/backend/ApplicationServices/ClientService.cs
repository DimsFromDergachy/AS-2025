using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.ApplicationServices;

public class ClientService
{
    private readonly IContext _context;

    public ClientService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Client>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
