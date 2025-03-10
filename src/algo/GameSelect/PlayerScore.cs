namespace Algo.GameSelect;

public record PlayerScore
{
    public string PlayerName { get; init; } = string.Empty;

    public GamePreference Preference { get; init; }

    public double Score { get; init; }
}