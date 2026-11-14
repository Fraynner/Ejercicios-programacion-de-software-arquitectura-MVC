using System.ComponentModel.DataAnnotations;

namespace RecipePlatform.Models;

public class Recipe
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Tipo de comida")]
    public MealType MealType { get; set; }

    [Required(ErrorMessage = "Agrega al menos un ingrediente.")]
    [Display(Name = "Ingredientes (uno por línea)")]
    public string Ingredients { get; set; } = string.Empty;

    [Required(ErrorMessage = "Las instrucciones son obligatorias.")]
    public string Instructions { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "El tiempo de preparación debe estar entre 1 y 1000 minutos.")]
    [Display(Name = "Tiempo de preparación (minutos)")]
    public int PrepMinutes { get; set; } = 30;

    // "Saved" = added to the user's favorites.
    public bool IsSaved { get; set; }

    // Public code used to share the recipe through a link.
    public string ShareCode { get; set; } = string.Empty;

    public IEnumerable<string> IngredientList =>
        Ingredients.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
