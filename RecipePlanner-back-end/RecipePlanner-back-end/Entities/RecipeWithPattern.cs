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
        public List<Recipe> RecipesPaggination { get; set; }

        public RecipeWithPattern()
        {
            RecipesWithPatternInName = new List<Recipe>();
            RecipesWithPatternInIngredient = new List<Recipe>();
            Count = 0;
            PageCount = 0;
            RecipesPaggination = new List<Recipe>();
        }

        public void CreatePaggination(int page)
        {
            this.RecipesWithPatternInName.AddRange(this.RecipesWithPatternInIngredient);

            this.RecipesWithPatternInName = this.RecipesWithPatternInName.DistinctBy(r => r.Name).ToList();

            int limit = 10;
            int skip = 0;

            if(page * limit - limit < this.RecipesWithPatternInName.Count)
            {
                skip += limit * page - limit;

                this.RecipesPaggination.AddRange(this.RecipesWithPatternInName.Skip(skip).Take(limit).ToList());
            }

            if ((this.RecipesWithPatternInName.Count % limit) == 0)
            {
                this.PageCount = this.RecipesWithPatternInName.Count / limit;
            }
            else
            {
                this.PageCount = this.RecipesWithPatternInName.Count / limit + 1;
            }

            this.Count = this.RecipesWithPatternInName.Count;
        }
    }
}
