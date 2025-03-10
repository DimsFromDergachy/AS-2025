namespace Algo.GameSelect;

public record Game
{
    public string Name { get; init; }

    public int MinPlayers { get; init; }

    public int MaxPlayers { get; init; }
}