using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entity_Relations.Model
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.Recipes = new HashSet<RecipeIngredients>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
       public ICollection<RecipeIngredients> Recipes { get; set; }
    }
}
