using AS_2025.Algos.Backpack;
using AS_2025.Algos.Common;

namespace AS_2025.Api.Algos.Backpack;

public record BackpackResponse
{
    public IReadOnlyCollection<Item> InputItems { get; init; } = new List<Item>();

    public AnnealingParameters Parameters { get; init; }
    
    public int BestValue { get; init; }
    
    public IReadOnlyCollection<Item> SelectedItems { get; init; } = new List<Item>();
}