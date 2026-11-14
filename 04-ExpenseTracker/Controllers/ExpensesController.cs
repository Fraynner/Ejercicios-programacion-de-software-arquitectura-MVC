using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

public class ExpensesController : Controller
{
    private readonly AppDbContext _db;

    public ExpensesController(AppDbContext db)
    {
        _db = db;
    }

    // month format: yyyy-MM (value of an <input type="month">)
    public async Task<IActionResult> Index(string? month)
    {
        var monthStart = DateTime.Today.AddDays(1 - DateTime.Today.Day);
        if (!string.IsNullOrEmpty(month) &&
            DateTime.TryParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            monthStart = parsed;
        }
        var monthEnd = monthStart.AddMonths(1);

        var expenses = await _db.Expenses
            .Where(e => e.Date >= monthStart && e.Date < monthEnd)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

        // Aggregate in memory (SQLite cannot sum decimal columns in SQL).
        var total = expenses.Sum(e => e.Amount);
        var byCategory = expenses
            .GroupBy(e => e.Category)
            .Select(g => new CategoryTotal
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount),
                Percent = total == 0 ? 0 : (double)(g.Sum(e => e.Amount) / total * 100m)
            })
            .OrderByDescending(c => c.Total)
            .ToList();

        return View(new ExpenseSummaryViewModel
        {
            Month = monthStart,
            Expenses = expenses,
            Total = total,
            ByCategory = byCategory
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Expense());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (!ModelState.IsValid)
        {
            return View(expense);
        }

        expense.Date = expense.Date.Date;
        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { month = expense.Date.ToString("yyyy-MM") });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? month)
    {
        var expense = await _db.Expenses.FindAsync(id);
        if (expense != null)
        {
            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index), new { month });
    }

    public IActionResult Error() => View();
}
