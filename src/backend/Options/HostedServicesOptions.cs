namespace AS_2025.Options;

public record HostedServicesOptions
{
    public static string ApplicationDbInitializer = "ApplicationDbInitializer";
    public static string ImportData = "ImportData";
    public static string IdentityInitializer = "IdentityInitializer";

    public Dictionary<string, bool> EnabledMap { get; init; } = new()
    {
        { ApplicationDbInitializer, true },
        { ImportData, true },
        { IdentityInitializer, true },
    };

    public bool IsEnabled(string hostedServiceName)
    {
        return EnabledMap.TryGetValue(hostedServiceName, out var enabled) && enabled;
    }
}