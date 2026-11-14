using System.ComponentModel.DataAnnotations;

namespace NotesManager.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}
