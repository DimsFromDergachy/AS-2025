namespace Algo.GameSelect;

public record GameEvaluation
{
    public Game Game { get; init; }

    public double Score { get; init; }

    public IReadOnlyCollection<PlayerScore> PlayerScores { get; init; } = new List<PlayerScore>();

    public EvaluationStatus Status { get; init; }

    public string Details { get; init; } = string.Empty;
}