using Xunit.Abstractions;

namespace Algo.GameSelect;

public class GameSelectorTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly List<Game> _games = new()
    {
        new Game
        {
            Name = "Монополия",
            MinPlayers = 2,
            MaxPlayers = 6
        },
        new Game
        {
            Name = "Уно", 
            MinPlayers = 2, 
            MaxPlayers = 10
        },
        new Game
        {
            Name = "Каркассон", 
            MinPlayers = 2, 
            MaxPlayers = 5
        }
    };

    private readonly List<PlayerPreferences> _players = new()
    {
        new PlayerPreferences
        {
            Name = "Алексей",
            WillingToTryNewGame = true,
            Preferences = new Dictionary<string, GamePreference>
            {
                { "Монополия", GamePreference.Pleasant },
                { "Каркассон", GamePreference.Favorite }
            }
        },
        new PlayerPreferences
        {
            Name = "Мария",
            WillingToTryNewGame = false,
            Preferences = new Dictionary<string, GamePreference>
            {
                { "Монополия", GamePreference.Undesirable },
                { "Уно", GamePreference.Pleasant }
            }
        }
    };

    public GameSelectorTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void CanSelectGame()
    {
        var gameSelector = new GameSelector(_games, _players);
        var selectResult = gameSelector.FindBestGame();
        _testOutputHelper.WriteLine($"Result: {selectResult}");
    }
}