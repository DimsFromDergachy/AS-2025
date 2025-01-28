namespace AS_2025.Import.Filesystem.Model;

public record Topic
{
    public string Code { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public IReadOnlyList<string> Lessons { get; init; } = Array.Empty<string>();

    public IReadOnlyList<string> Traits { get; init; } = Array.Empty<string>();
}