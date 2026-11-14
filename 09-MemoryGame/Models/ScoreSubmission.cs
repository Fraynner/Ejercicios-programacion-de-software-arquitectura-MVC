using System.ComponentModel.DataAnnotations;

namespace MemoryGame.Models;

public class ScoreSubmission
{
    [Required(ErrorMessage = "Escribe tu nombre.")]
    [StringLength(30)]
    public string PlayerName { get; set; } = string.Empty;

    public Difficulty Difficulty { get; set; }

    [Range(1, 10000)]
    public int Moves { get; set; }

    [Range(1, 86400)]
    public int Seconds { get; set; }
}
