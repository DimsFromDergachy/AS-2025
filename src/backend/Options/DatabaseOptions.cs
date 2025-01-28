namespace AS_2025.Options;

public record DatabaseOptions
{
    public string ConnectionString { get; init; } = string.Empty;
}