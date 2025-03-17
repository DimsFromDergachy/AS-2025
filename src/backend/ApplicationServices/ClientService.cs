using AS_2025.Api.Client.Delete;
using AS_2025.ApplicationServices.Filters;
using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.ApplicationServices;

public class ClientService
{
    private readonly IContext _context;

    public ClientService(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Client>> ListAsync(ListClientsFilter filter, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        return _context.Clients.Where(x => x.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
