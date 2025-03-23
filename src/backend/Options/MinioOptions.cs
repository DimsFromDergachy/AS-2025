namespace AS_2025.Options;

public record MinioOptions
{
    public string Endpoint { get; init; } = string.Empty;

    public int Port { get; init; }

    public string AccessKey { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public string Bucket { get; init; } = string.Empty;
}