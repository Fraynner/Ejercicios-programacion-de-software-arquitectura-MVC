using Microsoft.EntityFrameworkCore;
using Stopwatch.Models;

namespace Stopwatch.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LapRecord> Laps => Set<LapRecord>();
}
