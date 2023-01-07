using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class AdditionalInfo
    {
        public int Id { get; set; }
        public int IdMeal { get; set; }
        public int IdKindOfMeal { get; set; }
        public string CookingTime { get; set; } = null!;
        public string? Image { get; set; }
        public int? IdCuisine { get; set; }

        public virtual CuisineType? IdCuisineNavigation { get; set; }
        public virtual KindOfMeal IdKindOfMealNavigation { get; set; } = null!;
        public virtual MainTable IdMealNavigation { get; set; } = null!;
    }
}
