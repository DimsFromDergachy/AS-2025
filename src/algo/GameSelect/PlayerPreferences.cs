namespace Algo.GameSelect;

public record PlayerPreferences
{
    public string Name { get; init; } = string.Empty;

    public bool WillingToTryNewGame { get; init; }

    public IReadOnlyDictionary<string, GamePreference> Preferences { get; init; } = new Dictionary<string, GamePreference>();
}