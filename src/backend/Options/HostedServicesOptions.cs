namespace AS_2025.Options;

public record HostedServicesOptions
{
    public static string ApplicationDbInitializer = "ApplicationDbInitializer";

    public Dictionary<string, bool> EnabledMap { get; init; } = new()
    {
        { ApplicationDbInitializer, true }
    };

    public bool IsEnabled(string hostedServiceName)
    {
        return EnabledMap.TryGetValue(hostedServiceName, out var enabled) && enabled;
    }
}