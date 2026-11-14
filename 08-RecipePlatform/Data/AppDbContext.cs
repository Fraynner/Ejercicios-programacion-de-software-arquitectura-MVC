using Microsoft.EntityFrameworkCore;
using RecipePlatform.Models;

namespace RecipePlatform.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Recipe> Recipes => Set<Recipe>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>().HasData(
            new Recipe
            {
                Id = 1, Title = "Panqueques clásicos", MealType = MealType.Breakfast, PrepMinutes = 20,
                Ingredients = "2 tazas de harina\n2 huevos\n1.5 tazas de leche\n2 cucharadas de azúcar\n1 cucharada de polvo de hornear\nMantequilla",
                Instructions = "Mezcla los ingredientes secos. Agrega los huevos y la leche y bate hasta que quede suave. Cocina porciones en un sartén con mantequilla hasta que estén doradas por ambos lados.",
                ShareCode = "pancake1"
            },
            new Recipe
            {
                Id = 2, Title = "Pasta con tomate", MealType = MealType.Dinner, PrepMinutes = 25,
                Ingredients = "250 g de pasta\n4 tomates\n2 dientes de ajo\nAceite de oliva\nAlbahaca\nSal",
                Instructions = "Cocina la pasta. Sofríe el ajo en aceite de oliva, agrega los tomates picados y cocina 10 minutos. Mezcla con la pasta y decora con albahaca.",
                ShareCode = "pasta002"
            });
    }
}
