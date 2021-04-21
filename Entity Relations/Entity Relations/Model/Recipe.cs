using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entity_Relations.Model
{
    public class Recipe
    {
        public Recipe()
        {
            this.Ingredients = new HashSet<RecipeIngredients>();
        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan CookingTime { get; set; }

       public ICollection<RecipeIngredients> Ingredients { get; set; }
    }
}
