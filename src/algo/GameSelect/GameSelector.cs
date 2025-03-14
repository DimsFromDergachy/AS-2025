namespace Algo.GameSelect;

public class GameSelector
{
    private readonly List<Game> _games;
    private readonly List<PlayerPreferences> _players;

    public GameSelector(List<Game> games, List<PlayerPreferences> players)
    {
        _games = games;
        _players = players;
    }

    public (Game? SelectedGame, GameSelectionResult Result) FindBestGame()
    {
        var compatibleGames = FilterCompatibleGames();
        if (!compatibleGames.Any())
        {
            return (null, new GameSelectionResult
            {
                Status = SelectionStatus.NoCompatibleGames,
                Message = "Нет игр, подходящих по количеству игроков"
            });
        }

        var gameEvaluations = EvaluateGames(compatibleGames);
        if (!gameEvaluations.Any())
        {
            return (null, new GameSelectionResult
            {
                Status = SelectionStatus.NoSuitableGames,
                Message = "Невозможно найти устраивающую всех игру"
            });
        }

        var bestGame = SelectOptimalGame(gameEvaluations);
        return bestGame;
    }

    private List<Game> FilterCompatibleGames()
    {
        return _games.Where(g => g.MinPlayers <= _players.Count && g.MaxPlayers >= _players.Count).ToList();
    }

    private List<GameEvaluation> EvaluateGames(List<Game> games)
    {
        var result = new List<GameEvaluation>();

        foreach (var game in games)
        {
            var playerScores = EvaluateGameForPlayers(game);
            var gameScore = CalculateGameScore(playerScores);

            if (gameScore.Status != SelectionStatus.Selected)
            {
                continue;
            }

            result.Add(new GameEvaluation
            {
                Game = game,
                Score = gameScore.Score,
                PlayerScores = playerScores,
                Status = EvaluationStatus.Valid,
                Details = gameScore.Details
            });
        }

        return result;;
    }

    private List<PlayerScore> EvaluateGameForPlayers(Game game)
    {
        return _players.Select(player =>
        {
            var preference = GetPlayerGamePreference(player, game);
            return new PlayerScore
            {
                PlayerName = player.Name,
                Preference = preference,
                Score = CalculatePlayerScore(player, game, preference)
            };
        }).ToList();
    }

    private GamePreference GetPlayerGamePreference(PlayerPreferences player, Game game)
    {
        if (player.Preferences.TryGetValue(game.Name, out var preference))
        {
            return preference;
        }

        return player.WillingToTryNewGame ? GamePreference.Neutral : GamePreference.Hated;
    }

    private double CalculatePlayerScore(PlayerPreferences player, Game game, GamePreference preference)
    {
        return preference switch
        {
            GamePreference.Favorite => 2.0,
            GamePreference.Pleasant => 1.0,
            GamePreference.Neutral => 0.0,
            GamePreference.Undesirable => -0.5,
            GamePreference.Hated => -2.0,
            _ => 0.0
        };
    }

    private GameSelectionResult CalculateGameScore(List<PlayerScore> playerScores)
    {
        var diversityFactor = CalculateDiversityFactor(playerScores);
        var polarizationIndex = CalculatePolarizationIndex(playerScores);

        var totalScore = playerScores.Sum(p => p.Score);
        var averageScore = totalScore / playerScores.Count;

        var finalScore = totalScore * (1 - polarizationIndex) * diversityFactor;

        if (playerScores.Any(p => p.Preference == GamePreference.Hated))
        {
            return new GameSelectionResult
            {
                Status = SelectionStatus.Rejected,
                Score = double.MinValue,
                Message = "Есть игроки, которые ненавидят эту игру"
            };
        }

        return new GameSelectionResult
        {
            Status = SelectionStatus.Selected,
            Score = finalScore,
            Details = $"Средний балл: {averageScore:F2}, Индекс поляризации: {polarizationIndex:F2}"
        };
    }

    private double CalculateDiversityFactor(List<PlayerScore> playerScores)
    {
        var uniquePreferences = playerScores
            .Select(p => p.Preference)
            .Distinct()
            .Count();

        return 1 + (uniquePreferences / (double)_players.Count);
    }

    private double CalculatePolarizationIndex(List<PlayerScore> playerScores)
    {
        var scores = playerScores.Select(p => p.Score).ToList();
        var stdDev = CalculateStandardDeviation(scores);

        return Math.Min(stdDev / 2.0, 1.0);
    }

    private double CalculateStandardDeviation(List<double> values)
    {
        if (values.Count < 2)
        {
            return 0;
        }

        var mean = values.Average();
        var variance = values.Average(v => Math.Pow(v - mean, 2));

        return Math.Sqrt(variance);
    }

    private (Game SelectedGame, GameSelectionResult Result) SelectOptimalGame(List<GameEvaluation> evaluations)
    {
        var bestEvaluation = evaluations.MaxBy(e => e.Score);

        return (bestEvaluation.Game, new GameSelectionResult
        {
            Status = SelectionStatus.Selected,
            Score = bestEvaluation.Score,
            Message = $"Выбрана игра {bestEvaluation.Game.Name}",
            Details = bestEvaluation.Details
        });
    }
}