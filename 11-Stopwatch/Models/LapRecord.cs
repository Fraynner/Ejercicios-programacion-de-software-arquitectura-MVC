using System.ComponentModel.DataAnnotations;

namespace Stopwatch.Models;

public class LapRecord
{
    public int Id { get; set; }

    [Range(1, 100000)]
    public int LapNumber { get; set; }

    // Time of this lap alone.
    [Range(0, long.MaxValue)]
    public long LapMilliseconds { get; set; }

    // Total elapsed time when the lap button was pressed.
    [Range(0, long.MaxValue)]
    public long TotalMilliseconds { get; set; }

    public DateTime RecordedAt { get; set; } = DateTime.Now;
}
