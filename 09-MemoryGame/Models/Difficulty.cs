using System.ComponentModel.DataAnnotations;

namespace MemoryGame.Models;

public enum Difficulty
{
    [Display(Name = "Fácil")]
    Easy,
    [Display(Name = "Medio")]
    Medium,
    [Display(Name = "Difícil")]
    Hard
}

public static class DifficultyExtensions
{
    public static int PairCount(this Difficulty difficulty) => difficulty switch
    {
        Difficulty.Easy => 6,
        Difficulty.Medium => 8,
        _ => 12
    };

    public static int GridColumns(this Difficulty difficulty) => difficulty switch
    {
        Difficulty.Easy => 4,
        Difficulty.Medium => 4,
        _ => 6
    };
}
