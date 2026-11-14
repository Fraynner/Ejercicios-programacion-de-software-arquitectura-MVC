using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public enum ExpenseCategory
{
    [Display(Name = "Comida")]
    Food,
    [Display(Name = "Transporte")]
    Transport,
    [Display(Name = "Vivienda")]
    Housing,
    [Display(Name = "Salud")]
    Health,
    [Display(Name = "Entretenimiento")]
    Entertainment,
    [Display(Name = "Educación")]
    Education,
    [Display(Name = "Otros")]
    Other
}
