using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Common;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DatabaseFacade Database { get; }

    public DbSet<Trait> Traits { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}