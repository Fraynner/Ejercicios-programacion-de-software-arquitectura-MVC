using System.ComponentModel.DataAnnotations;

namespace RecipePlatform.Models;

public enum MealType
{
    [Display(Name = "Desayuno")]
    Breakfast,
    [Display(Name = "Almuerzo")]
    Lunch,
    [Display(Name = "Cena")]
    Dinner,
    [Display(Name = "Postre")]
    Dessert,
    [Display(Name = "Merienda")]
    Snack,
    [Display(Name = "Vegetariano")]
    Vegetarian
}
