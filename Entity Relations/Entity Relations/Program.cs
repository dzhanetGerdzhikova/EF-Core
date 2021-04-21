using System;
using System.Collections.Generic;
using Entity_Relations.Model;
namespace Entity_Relations
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataBase = new RecipesDbContext();
            dataBase.Database.EnsureDeleted();
            dataBase.Database.EnsureCreated();
            Recipe recipe = new Recipe
            {
                Name = "Musaka",
                Description = "Traditional Bulgarian meal.",
                CookingTime = new TimeSpan(2, 3, 4),
                Ingredients =new HashSet<RecipeIngredients>
                {
                    new RecipeIngredients{Ingredient= new Ingredient {Name="Pottato"}, Quantity=1000},
                    new RecipeIngredients{Ingredient=new Ingredient{Name="Mear"},Quantity=2000}
                }
            };
            dataBase.Recipes.Add(recipe);
            dataBase.SaveChanges();
        }
    }
}
