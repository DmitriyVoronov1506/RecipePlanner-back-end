using System.Text.Json.Serialization;

namespace RecipePlanner_back_end.Entities
{
    public class RecipeWithPattern
    {
        [JsonIgnore]
        public List<Recipe> RecipesWithPatternInName { get; set; }

        [JsonIgnore]
        public List<Recipe> RecipesWithPatternInIngredient { get; set; }

        public int? Count { get; set; }
        public int? PageCount { get; set; }
        public Dictionary<int, List<Recipe>> RecipesPaggination { get; set; }

        public RecipeWithPattern()
        {
            RecipesWithPatternInName = new List<Recipe>();
            RecipesWithPatternInIngredient = new List<Recipe>();
            Count = 0;
            PageCount = 0;
            RecipesPaggination = new Dictionary<int, List<Recipe>>();
        }

        public void CreatePaggination()
        {
            this.RecipesWithPatternInName.AddRange(this.RecipesWithPatternInIngredient);

            int skip = 0;
            int take = 10;
            int count = 1;

            while (skip < this.RecipesWithPatternInName.Count())
            {
                this.RecipesPaggination.Add(count, this.RecipesWithPatternInName.Skip(skip).Take(take).ToList());

                skip += 10;
                count++;
            }

            this.Count = this.RecipesWithPatternInName.Count;
            this.PageCount = this.RecipesPaggination.Count;
        }
    }
}
