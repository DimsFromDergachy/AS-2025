namespace AS_2025.Import.Filesystem.Model;

public record Lesson
{
    public string Code { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public IReadOnlyList<string> Traits { get; init; } = Array.Empty<string>();

    public IReadOnlyList<Supplement> Supplement { get; init; } = Array.Empty<Supplement>();

    public IReadOnlyList<string> Tasks { get; init; } = Array.Empty<string>();

    public string Author { get; init; } = string.Empty;
}