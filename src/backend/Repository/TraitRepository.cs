using AS_2025.Domain;
using Task = System.Threading.Tasks.Task;

namespace AS_2025.Repository;

public class TraitRepository : ITraitRepository
{
    private readonly Dictionary<Guid, Trait> _cache = new();

    public async Task SaveAsync(Trait trait, CancellationToken cancellationToken)
    {
        _cache[trait.Id] = trait;
    }

    public async Task<Trait?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return _cache.TryGetValue(id, out var value) ? value : null;
    }
}