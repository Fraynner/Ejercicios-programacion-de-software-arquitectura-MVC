namespace MemoryGame.Models;

public class PlayViewModel
{
    public Difficulty Difficulty { get; set; }
    public List<Card> Cards { get; set; } = new();
}

public class GameIndexViewModel
{
    public List<GameScore> TopScores { get; set; } = new();
}
