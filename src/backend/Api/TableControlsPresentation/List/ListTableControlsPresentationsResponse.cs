
namespace AS_2025.Api.TableControlsPresentation.List;

public record ListTableControlsPresentationsResponse
{
    public IReadOnlyCollection<ListTableControlsPresentationsItem> Items { get; init; } = Array.Empty<ListTableControlsPresentationsItem>();
}
