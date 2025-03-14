
namespace AS_2025.Api.Project.List;

public record ListProjectsResponse
{
    public IReadOnlyCollection<ListProjectsItem> Items { get; init; } = Array.Empty<ListProjectsItem>();
}
