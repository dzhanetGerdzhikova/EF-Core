using Entity_Relations.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity_Relations.Model
{
    public class RecipesDbContext : DbContext
    {
        public RecipesDbContext()
        {

        }

        public RecipesDbContext(DbContextOptions<RecipesDbContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Recipes;Integrated Security=true;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecipeConfigurations());
            modelBuilder.Entity<RecipeIngredients>().HasKey(x=> new { x.RecipeId, x.IngredientId });
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
