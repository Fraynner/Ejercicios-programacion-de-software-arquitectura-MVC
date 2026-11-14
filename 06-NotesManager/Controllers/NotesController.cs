using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesManager.Data;
using NotesManager.Models;

namespace NotesManager.Controllers;

public class NotesController : Controller
{
    private readonly AppDbContext _db;

    public NotesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId)
    {
        var query = _db.Notes.Include(n => n.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(n => EF.Functions.Like(n.Title, pattern) || EF.Functions.Like(n.Content, pattern));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(n => n.CategoryId == categoryId);
        }

        return View(new NotesIndexViewModel
        {
            Notes = await query.OrderByDescending(n => n.UpdatedAt).ToListAsync(),
            Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync(),
            Search = search,
            CategoryId = categoryId
        });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync(null);
        return View("Form", new Note());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Content,CategoryId")] Note note)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(note.CategoryId);
            return View("Form", note);
        }

        note.CreatedAt = DateTime.Now;
        note.UpdatedAt = DateTime.Now;
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note == null) return NotFound();

        await LoadCategoriesAsync(note.CategoryId);
        return View("Form", note);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CategoryId")] Note input)
    {
        if (id != input.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(input.CategoryId);
            return View("Form", input);
        }

        var note = await _db.Notes.FindAsync(id);
        if (note == null) return NotFound();

        note.Title = input.Title;
        note.Content = input.Content;
        note.CategoryId = input.CategoryId;
        note.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note != null)
        {
            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();

    private async Task LoadCategoriesAsync(int? selected)
    {
        var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selected);
    }
}
