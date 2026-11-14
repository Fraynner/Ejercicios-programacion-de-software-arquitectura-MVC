using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Controllers;

public class TodosController : Controller
{
    private readonly AppDbContext _db;

    public TodosController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        return View(await BuildViewModelAsync(new TodoItem()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoListViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", await BuildViewModelAsync(vm.NewItem));
        }

        vm.NewItem.Title = vm.NewItem.Title.Trim();
        vm.NewItem.CreatedAt = DateTime.Now;
        _db.TodoItems.Add(vm.NewItem);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var item = await _db.TodoItems.FindAsync(id);
        if (item != null)
        {
            item.IsCompleted = !item.IsCompleted;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.TodoItems.FindAsync(id);
        if (item != null)
        {
            _db.TodoItems.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();

    private async Task<TodoListViewModel> BuildViewModelAsync(TodoItem newItem)
    {
        var items = await _db.TodoItems.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.Id).ToListAsync();
        return new TodoListViewModel { Items = items, NewItem = newItem };
    }
}
