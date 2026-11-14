using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesManager.Data;
using NotesManager.Models;

namespace NotesManager.Controllers;

public class CategoriesController : Controller
{
    private readonly AppDbContext _db;

    public CategoriesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _db.Categories.OrderBy(c => c.Name).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Category category)
    {
        if (ModelState.IsValid)
        {
            var exists = await _db.Categories.AnyAsync(c => c.Name == category.Name.Trim());
            if (exists)
            {
                TempData["Error"] = "Esa categoría ya existe.";
            }
            else
            {
                category.Name = category.Name.Trim();
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
            }
        }
        else
        {
            TempData["Error"] = "El nombre de la categoría es obligatorio.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category != null)
        {
            // Keep the notes, just remove their category.
            var notes = await _db.Notes.Where(n => n.CategoryId == id).ToListAsync();
            foreach (var n in notes) n.CategoryId = null;

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();
}
