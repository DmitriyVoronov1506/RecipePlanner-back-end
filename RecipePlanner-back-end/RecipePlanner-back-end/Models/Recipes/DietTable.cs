using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class DietTable
    {
        public DietTable()
        {
            DietMeals = new HashSet<DietMeal>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<DietMeal> DietMeals { get; set; }
    }
}
