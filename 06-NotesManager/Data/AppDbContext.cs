using Microsoft.EntityFrameworkCore;
using NotesManager.Models;

namespace NotesManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Personal" },
            new Category { Id = 2, Name = "Trabajo" },
            new Category { Id = 3, Name = "Estudio" });
    }
}
