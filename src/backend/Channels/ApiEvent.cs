namespace AS_2025.Channels;

public record ApiEvent
{
    public string Path { get; init; } = string.Empty;

    public string Method { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; }
}