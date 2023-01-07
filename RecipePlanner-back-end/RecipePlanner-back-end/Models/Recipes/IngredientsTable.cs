using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class IngredientsTable
    {
        public IngredientsTable()
        {
            IngredientAlergens = new HashSet<IngredientAlergen>();
            MealIngredients = new HashSet<MealIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<IngredientAlergen> IngredientAlergens { get; set; }
        public virtual ICollection<MealIngredient> MealIngredients { get; set; }
    }
}
