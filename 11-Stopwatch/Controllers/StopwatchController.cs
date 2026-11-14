using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopwatch.Data;
using Stopwatch.Models;

namespace Stopwatch.Controllers;

public class StopwatchController : Controller
{
    private readonly AppDbContext _db;

    public StopwatchController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Saved laps, newest first (loaded by the page with fetch).
    [HttpGet]
    public async Task<IActionResult> Laps()
    {
        var laps = await _db.Laps
            .OrderByDescending(l => l.Id)
            .Take(100)
            .ToListAsync();

        return Json(laps.Select(l => new
        {
            lapNumber = l.LapNumber,
            lapMilliseconds = l.LapMilliseconds,
            totalMilliseconds = l.TotalMilliseconds,
            recordedAt = l.RecordedAt.ToString("yyyy-MM-dd HH:mm:ss")
        }));
    }

    [HttpPost]
    public async Task<IActionResult> SaveLap([FromBody] LapRecord lap)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        lap.Id = 0;
        lap.RecordedAt = DateTime.Now;
        _db.Laps.Add(lap);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> ClearHistory()
    {
        _db.Laps.RemoveRange(_db.Laps);
        await _db.SaveChangesAsync();
        return Ok();
    }

    public IActionResult Error() => View();
}
