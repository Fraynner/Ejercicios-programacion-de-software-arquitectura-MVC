namespace NotesManager.Models;

public class NotesIndexViewModel
{
    public List<Note> Notes { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
}
