using System.ComponentModel.DataAnnotations;

namespace EventCalendar.Models;

public class CalendarEvent
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Inicio")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime StartTime { get; set; } = DateTime.Now;

    // Null means "no reminder".
    [Display(Name = "Recordatorio")]
    public int? ReminderMinutesBefore { get; set; }
}
