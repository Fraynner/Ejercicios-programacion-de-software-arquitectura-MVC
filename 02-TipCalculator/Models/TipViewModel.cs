namespace TipCalculator.Models;

public class TipViewModel
{
    public TipInput Input { get; set; } = new();
    public TipResult? Result { get; set; }
}
