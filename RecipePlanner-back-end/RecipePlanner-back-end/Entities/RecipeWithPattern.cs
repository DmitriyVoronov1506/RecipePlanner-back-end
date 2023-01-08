namespace RecipePlanner_back_end.Entities
{
    public class RecipeWithPattern
    {
        public List<Recipe> RecipesWithPatternInName { get; set; }
        public List<Recipe> RecipesWithPatternInIngredient { get; set; }

        public int? Count { get; set; }

        public RecipeWithPattern()
        {
            RecipesWithPatternInName = new List<Recipe>();
            RecipesWithPatternInIngredient = new List<Recipe>();
            Count = 0;
        }
    }
}
