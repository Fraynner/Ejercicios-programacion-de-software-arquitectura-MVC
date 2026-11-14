namespace RecipePlatform.Models;

public class RecipeIndexViewModel
{
    public List<Recipe> Recipes { get; set; } = new();
    public string? Search { get; set; }
    public MealType? MealType { get; set; }
    public string? Ingredients { get; set; }
    public bool SavedOnly { get; set; }
}
