using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class DietMeal
    {
        public int Id { get; set; }
        public int IdMeal { get; set; }
        public int IdDiet { get; set; }

        public virtual DietTable IdDietNavigation { get; set; } = null!;
        public virtual MainTable IdMealNavigation { get; set; } = null!;
    }
}
