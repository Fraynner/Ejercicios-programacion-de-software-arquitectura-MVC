using System.ComponentModel.DataAnnotations;

namespace PasswordGenerator.Models;

public class PasswordOptions : IValidatableObject
{
    [Range(4, 128, ErrorMessage = "La longitud debe estar entre 4 y 128.")]
    public int Length { get; set; } = 16;

    [Display(Name = "Letras mayúsculas (A-Z)")]
    public bool IncludeUppercase { get; set; } = true;

    [Display(Name = "Letras minúsculas (a-z)")]
    public bool IncludeLowercase { get; set; } = true;

    [Display(Name = "Números (0-9)")]
    public bool IncludeNumbers { get; set; } = true;

    [Display(Name = "Símbolos (!@#$...)")]
    public bool IncludeSymbols { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!IncludeUppercase && !IncludeLowercase && !IncludeNumbers && !IncludeSymbols)
        {
            yield return new ValidationResult(
                "Selecciona al menos un tipo de carácter.",
                new[] { nameof(IncludeUppercase) });
        }
    }
}
