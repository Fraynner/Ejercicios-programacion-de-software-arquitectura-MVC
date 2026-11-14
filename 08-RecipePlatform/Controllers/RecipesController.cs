using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipePlatform.Data;
using RecipePlatform.Models;

namespace RecipePlatform.Controllers;

public class RecipesController : Controller
{
    private readonly AppDbContext _db;

    public RecipesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? search, MealType? mealType, string? ingredients, bool savedOnly = false)
    {
        var query = _db.Recipes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(r => EF.Functions.Like(r.Title, pattern));
        }
        if (mealType.HasValue)
        {
            query = query.Where(r => r.MealType == mealType.Value);
        }
        if (savedOnly)
        {
            query = query.Where(r => r.IsSaved);
        }
        if (!string.IsNullOrWhiteSpace(ingredients))
        {
            // Comma separated list: the recipe must contain ALL of them.
            foreach (var term in ingredients.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var pattern = $"%{term}%";
                query = query.Where(r => EF.Functions.Like(r.Ingredients, pattern));
            }
        }

        return View(new RecipeIndexViewModel
        {
            Recipes = await query.OrderBy(r => r.Title).ToListAsync(),
            Search = search,
            MealType = mealType,
            Ingredients = ingredients,
            SavedOnly = savedOnly
        });
    }

    public async Task<IActionResult> Details(int id)
    {
        var recipe = await _db.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();
        return View(recipe);
    }

    // Public page opened from a shared link.
    [HttpGet("Recipes/Shared/{code}")]
    public async Task<IActionResult> Shared(string code)
    {
        var recipe = await _db.Recipes.FirstOrDefaultAsync(r => r.ShareCode == code);
        if (recipe == null) return NotFound();
        return View("Details", recipe);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Recipe());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,MealType,Ingredients,Instructions,PrepMinutes")] Recipe recipe)
    {
        if (!ModelState.IsValid)
        {
            return View(recipe);
        }

        recipe.ShareCode = Guid.NewGuid().ToString("N").Substring(0, 8);
        _db.Recipes.Add(recipe);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = recipe.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleSaved(int id, string? returnTo)
    {
        var recipe = await _db.Recipes.FindAsync(id);
        if (recipe != null)
        {
            recipe.IsSaved = !recipe.IsSaved;
            await _db.SaveChangesAsync();
        }

        if (returnTo == "details")
        {
            return RedirectToAction(nameof(Details), new { id });
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var recipe = await _db.Recipes.FindAsync(id);
        if (recipe != null)
        {
            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();
}
