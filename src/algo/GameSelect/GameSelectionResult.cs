namespace Algo.GameSelect;

public record GameSelectionResult
{
    public SelectionStatus Status { get; init; }

    public double Score { get; init; }

    public string Message { get; init; } = string.Empty;

    public string Details { get; init; } = string.Empty;
}