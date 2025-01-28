namespace AS_2025.Options;

public record ApplicationOptions
{
    public static string SectionKey = "Application";

    public DatabaseOptions Database { get; set; } = new();

    public HostedServicesOptions HostedServices { get; init; } = new();
}