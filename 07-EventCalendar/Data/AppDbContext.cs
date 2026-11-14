using Microsoft.EntityFrameworkCore;
using EventCalendar.Models;

namespace EventCalendar.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CalendarEvent> Events => Set<CalendarEvent>();
}
