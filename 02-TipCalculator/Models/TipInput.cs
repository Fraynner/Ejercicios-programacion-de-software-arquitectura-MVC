using System.ComponentModel.DataAnnotations;

namespace TipCalculator.Models;

public class TipInput
{
    [Required(ErrorMessage = "Ingresa el monto de la cuenta.")]
    [Range(0.01, 1000000, ErrorMessage = "La cuenta debe estar entre 0.01 y 1,000,000.")]
    [Display(Name = "Monto de la cuenta")]
    public decimal? BillAmount { get; set; }

    [Range(0, 100, ErrorMessage = "La propina debe estar entre 0% y 100%.")]
    [Display(Name = "Porcentaje de propina")]
    public decimal TipPercentage { get; set; } = 15;

    [Range(1, 50, ErrorMessage = "El número de personas debe estar entre 1 y 50.")]
    [Display(Name = "Número de personas")]
    public int People { get; set; } = 1;
}
