using System;
using System.Collections.Generic;
using System.Text;

namespace Entity_Relations.Model
{
  public   class RecipeIngredients
    {
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
    }

}
