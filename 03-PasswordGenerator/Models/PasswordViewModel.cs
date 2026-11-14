namespace PasswordGenerator.Models;

public class PasswordViewModel
{
    public PasswordOptions Options { get; set; } = new();
    public string? Password { get; set; }
    public string? Strength { get; set; }
}
