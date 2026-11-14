namespace EventCalendar.Models;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public bool InCurrentMonth { get; set; }
    public List<CalendarEvent> Events { get; set; } = new();
}

public class CalendarViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy", UiHelpers.Es);
    public DateTime PreviousMonth => new DateTime(Year, Month, 1).AddMonths(-1);
    public DateTime NextMonth => new DateTime(Year, Month, 1).AddMonths(1);
    public List<List<CalendarDay>> Weeks { get; set; } = new();
}
