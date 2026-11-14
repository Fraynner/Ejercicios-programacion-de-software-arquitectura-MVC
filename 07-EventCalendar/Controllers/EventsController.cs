using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventCalendar.Data;
using EventCalendar.Models;

namespace EventCalendar.Controllers;

public class EventsController : Controller
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(int? year, int? month)
    {
        var today = DateTime.Today;
        var y = year ?? today.Year;
        var m = month ?? today.Month;
        if (y < 1900 || y > 2200 || m < 1 || m > 12)
        {
            y = today.Year;
            m = today.Month;
        }

        var firstOfMonth = new DateTime(y, m, 1);
        var gridStart = firstOfMonth.AddDays(-(int)firstOfMonth.DayOfWeek); // weeks start on Sunday
        var gridEnd = gridStart.AddDays(42);

        var events = await _db.Events
            .Where(e => e.StartTime >= gridStart && e.StartTime < gridEnd)
            .OrderBy(e => e.StartTime)
            .ToListAsync();

        var vm = new CalendarViewModel { Year = y, Month = m };
        for (var w = 0; w < 6; w++)
        {
            var week = new List<CalendarDay>();
            for (var d = 0; d < 7; d++)
            {
                var date = gridStart.AddDays(w * 7 + d);
                week.Add(new CalendarDay
                {
                    Date = date,
                    InCurrentMonth = date.Month == m,
                    Events = events.Where(e => e.StartTime.Date == date).ToList()
                });
            }
            vm.Weeks.Add(week);
        }
        return View(vm);
    }

    [HttpGet]
    public IActionResult Create(DateTime? date)
    {
        var start = (date ?? DateTime.Today).Date.AddHours(9);
        return View("Form", new CalendarEvent { StartTime = start, ReminderMinutesBefore = 15 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,StartTime,ReminderMinutesBefore")] CalendarEvent calendarEvent)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", calendarEvent);
        }

        _db.Events.Add(calendarEvent);
        await _db.SaveChangesAsync();
        return RedirectToMonth(calendarEvent.StartTime);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var calendarEvent = await _db.Events.FindAsync(id);
        if (calendarEvent == null) return NotFound();
        return View("Form", calendarEvent);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartTime,ReminderMinutesBefore")] CalendarEvent input)
    {
        if (id != input.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            return View("Form", input);
        }

        var calendarEvent = await _db.Events.FindAsync(id);
        if (calendarEvent == null) return NotFound();

        calendarEvent.Title = input.Title;
        calendarEvent.Description = input.Description;
        calendarEvent.StartTime = input.StartTime;
        calendarEvent.ReminderMinutesBefore = input.ReminderMinutesBefore;
        await _db.SaveChangesAsync();
        return RedirectToMonth(calendarEvent.StartTime);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var calendarEvent = await _db.Events.FindAsync(id);
        if (calendarEvent == null) return RedirectToAction(nameof(Index));

        var start = calendarEvent.StartTime;
        _db.Events.Remove(calendarEvent);
        await _db.SaveChangesAsync();
        return RedirectToMonth(start);
    }

    // Polled by the browser every 30 seconds to show reminders.
    [HttpGet]
    public async Task<IActionResult> Upcoming()
    {
        var now = DateTime.Now;
        var candidates = await _db.Events
            .Where(e => e.ReminderMinutesBefore != null && e.StartTime > now)
            .ToListAsync();

        var due = candidates
            .Where(e => e.StartTime.AddMinutes(-e.ReminderMinutesBefore!.Value) <= now)
            .Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.StartTime.ToString("yyyy-MM-ddTHH:mm"),
                minutesLeft = (int)Math.Ceiling((e.StartTime - now).TotalMinutes)
            });

        return Json(due);
    }

    public IActionResult Error() => View();

    private IActionResult RedirectToMonth(DateTime date)
    {
        return RedirectToAction(nameof(Index), new { year = date.Year, month = date.Month });
    }
}
