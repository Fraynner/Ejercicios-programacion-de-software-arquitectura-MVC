using System.ComponentModel.DataAnnotations;

namespace MemoryGame.Models;

public class GameScore
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe tu nombre.")]
    [StringLength(30)]
    public string PlayerName { get; set; } = string.Empty;

    public Difficulty Difficulty { get; set; }
    public int Moves { get; set; }
    public int Seconds { get; set; }
    public int Points { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.Now;
}
