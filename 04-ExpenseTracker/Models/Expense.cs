using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public class Expense
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(100, ErrorMessage = "La descripción no puede superar los 100 caracteres.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 100000000, ErrorMessage = "El monto debe ser mayor que 0.")]
    public decimal Amount { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; } = DateTime.Today;

    public ExpenseCategory Category { get; set; }
}
