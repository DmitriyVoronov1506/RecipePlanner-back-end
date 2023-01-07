using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class Alergen
    {
        public Alergen()
        {
            IngredientAlergens = new HashSet<IngredientAlergen>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<IngredientAlergen> IngredientAlergens { get; set; }
    }
}
