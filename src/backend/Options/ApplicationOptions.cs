namespace AS_2025.Options;

public record ApplicationOptions
{
    public static string SectionKey = "Application";

    public DatabaseOptions Database { get; init; } = new();

    public HostedServicesOptions HostedServices { get; init; } = new();

    public DataOptions Data { get; init; } = new();

    public int FrontendOriginPort { get; init; } = 5004;
    public int FrontendLocalhostPort { get; init; } = 5173;
}