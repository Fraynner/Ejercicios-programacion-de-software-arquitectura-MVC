using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using BookingSystem.Models;

namespace BookingSystem.Controllers;

public class BookingController : Controller
{
    private static readonly string[] AllSlots =
        { "09:00", "10:00", "11:00", "12:00", "14:00", "15:00", "16:00" };

    private readonly AppDbContext _db;

    public BookingController(AppDbContext db)
    {
        _db = db;
    }

    // Step 1: choose a service
    public async Task<IActionResult> Index()
    {
        return View(await _db.Services.OrderBy(s => s.Name).ToListAsync());
    }

    // Step 2: choose a date and an available time slot
    [HttpGet]
    public async Task<IActionResult> Book(int serviceId, DateTime? date)
    {
        var service = await _db.Services.FindAsync(serviceId);
        if (service == null) return NotFound();

        var day = (date ?? DateTime.Today).Date;
        if (day < DateTime.Today) day = DateTime.Today;

        return View(new BookingFormViewModel
        {
            ServiceId = service.Id,
            ServiceName = service.Name,
            Date = day,
            AvailableSlots = await GetAvailableSlotsAsync(day)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(BookingFormViewModel vm)
    {
        var service = await _db.Services.FindAsync(vm.ServiceId);
        if (service == null) return NotFound();

        vm.ServiceName = service.Name;
        vm.Date = vm.Date.Date;

        if (vm.Date < DateTime.Today)
        {
            ModelState.AddModelError(nameof(vm.Date), "La fecha no puede estar en el pasado.");
        }

        vm.AvailableSlots = await GetAvailableSlotsAsync(vm.Date);
        if (vm.TimeSlot != null && !vm.AvailableSlots.Contains(vm.TimeSlot))
        {
            ModelState.AddModelError(nameof(vm.TimeSlot), "Ese horario ya no está disponible.");
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var booking = new Booking
        {
            ServiceItemId = service.Id,
            CustomerName = vm.CustomerName.Trim(),
            Email = vm.Email.Trim(),
            Date = vm.Date,
            TimeSlot = vm.TimeSlot!,
            Status = BookingStatus.Pending
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = booking.Id });
    }

    // Step 3: review and confirm
    public async Task<IActionResult> Details(int id)
    {
        var booking = await _db.Bookings.Include(b => b.ServiceItem).FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return NotFound();
        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int id)
    {
        var booking = await _db.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        if (booking.Status == BookingStatus.Pending)
        {
            booking.Status = BookingStatus.Confirmed;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _db.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        booking.Status = BookingStatus.Cancelled;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(MyBookings));
    }

    public async Task<IActionResult> MyBookings()
    {
        var bookings = await _db.Bookings
            .Include(b => b.ServiceItem)
            .OrderByDescending(b => b.Date)
            .ThenBy(b => b.TimeSlot)
            .ToListAsync();
        return View(bookings);
    }

    public IActionResult Error() => View();

    private async Task<List<string>> GetAvailableSlotsAsync(DateTime day)
    {
        var taken = await _db.Bookings
            .Where(b => b.Date == day && b.Status != BookingStatus.Cancelled)
            .Select(b => b.TimeSlot)
            .ToListAsync();

        var slots = AllSlots.Where(s => !taken.Contains(s));

        if (day == DateTime.Today)
        {
            var now = DateTime.Now.TimeOfDay;
            slots = slots.Where(s => TimeSpan.Parse(s) > now);
        }
        return slots.ToList();
    }
}
