using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models;

public class BookingFormViewModel
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Tu nombre es obligatorio.")]
    [StringLength(80)]
    [Display(Name = "Nombre completo")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tu correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Por favor elige un horario.")]
    [Display(Name = "Horario")]
    public string? TimeSlot { get; set; }

    public List<string> AvailableSlots { get; set; } = new();
}
