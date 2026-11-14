using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models;

public enum BookingStatus
{
    [Display(Name = "Pendiente")]
    Pending,
    [Display(Name = "Confirmada")]
    Confirmed,
    [Display(Name = "Cancelada")]
    Cancelled
}

public class Booking
{
    public int Id { get; set; }
    public int ServiceItemId { get; set; }
    public ServiceItem? ServiceItem { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
