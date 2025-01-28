namespace AS_2025.Import.Filesystem.Model;

public record Task
{
    public string Code { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public IReadOnlyList<Supplement> Supplement { get; init; } = Array.Empty<Supplement>();

    public int? Difficulty { get; init; }

    public int? Time { get; init; }
}