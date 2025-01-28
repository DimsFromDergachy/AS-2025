using AS_2025.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.Repository;

public interface ITraitRepository
{
    Task SaveAsync(Trait trait, CancellationToken cancellationToken);

    Task<Trait?> GetAsync(Guid id, CancellationToken cancellationToken);
}