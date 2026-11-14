namespace TipCalculator.Models;

public class TipResult
{
    public decimal Bill { get; init; }
    public decimal TipPercentage { get; init; }
    public decimal TipAmount { get; init; }
    public decimal Total { get; init; }
    public int People { get; init; }
    public decimal TipPerPerson { get; init; }
    public decimal TotalPerPerson { get; init; }

    public static TipResult Calculate(decimal bill, decimal percentage, int people)
    {
        var tip = Math.Round(bill * percentage / 100m, 2);
        var total = bill + tip;
        return new TipResult
        {
            Bill = bill,
            TipPercentage = percentage,
            TipAmount = tip,
            Total = total,
            People = people,
            TipPerPerson = Math.Round(tip / people, 2),
            TotalPerPerson = Math.Round(total / people, 2)
        };
    }
}
