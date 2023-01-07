using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class MealIngredient
    {
        public int Id { get; set; }
        public int IdMeal { get; set; }
        public int IdIngredient { get; set; }
        public string Quantity { get; set; } = null!;

        public virtual IngredientsTable IdIngredientNavigation { get; set; } = null!;
        public virtual MainTable IdMealNavigation { get; set; } = null!;
    }
}
