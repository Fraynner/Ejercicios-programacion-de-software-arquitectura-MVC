using Microsoft.EntityFrameworkCore;
using MemoryGame.Models;

namespace MemoryGame.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<GameScore> Scores => Set<GameScore>();
}
