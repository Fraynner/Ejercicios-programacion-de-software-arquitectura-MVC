namespace ExpenseTracker.Models;

public class CategoryTotal
{
    public ExpenseCategory Category { get; set; }
    public decimal Total { get; set; }
    public double Percent { get; set; }
}

public class ExpenseSummaryViewModel
{
    public DateTime Month { get; set; }
    public List<Expense> Expenses { get; set; } = new();
    public decimal Total { get; set; }
    public List<CategoryTotal> ByCategory { get; set; } = new();
}
