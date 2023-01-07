using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class IngredientAlergen
    {
        public int Id { get; set; }
        public int IdIngredient { get; set; }
        public int IdAlergens { get; set; }

        public virtual Alergen IdAlergensNavigation { get; set; } = null!;
        public virtual IngredientsTable IdIngredientNavigation { get; set; } = null!;
    }
}
