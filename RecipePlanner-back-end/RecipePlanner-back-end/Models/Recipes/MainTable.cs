using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class MainTable
    {
        public MainTable()
        {
            AdditionalInfos = new HashSet<AdditionalInfo>();
            DietMeals = new HashSet<DietMeal>();
            MealIngredients = new HashSet<MealIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Calories { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; }

        [JsonIgnore]
        public virtual ICollection<DietMeal> DietMeals { get; set; }

        [JsonIgnore]
        public virtual ICollection<MealIngredient> MealIngredients { get; set; }
    }
}
