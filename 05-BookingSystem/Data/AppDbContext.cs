using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;

namespace BookingSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ServiceItem> Services => Set<ServiceItem>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceItem>().HasData(
            new ServiceItem { Id = 1, Name = "Corte de cabello", Description = "Corte de cabello clásico y peinado.", DurationMinutes = 45, Price = 20m },
            new ServiceItem { Id = 2, Name = "Revisión dental", Description = "Revisión general y limpieza.", DurationMinutes = 60, Price = 45m },
            new ServiceItem { Id = 3, Name = "Masaje", Description = "Masaje relajante de cuerpo completo.", DurationMinutes = 60, Price = 55m });
    }
}
