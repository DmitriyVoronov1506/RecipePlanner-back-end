using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Entities;
using RecipePlanner_back_end.Models.Recipes;
using RecipePlanner_back_end.Services;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace RecipePlanner_back_end.Controllers
{
    [ApiController]
    [Route("RecipesFilterController")]
    public class RecipesFilterController : ControllerBase
    {
        private readonly RecipeDatabaseContext _recipeDatabaseContext;
        private readonly DevelopeService _developeService;

        List<AdditionalInfo> addinfo = null!;
        List<MainTable> maintables = null!;
        List<IngredientsTable> ingredientsTables = null!;
        List<CuisineType> cuisineTypes = null!;
        List<KindOfMeal> kindOfMeals = null!;
        List<DietTable> dietTables = null!;
        List<DietMeal> dietMeals = null!;
        List<MealIngredient> mealIngredients = null!;

        public RecipesFilterController(RecipeDatabaseContext recipeDatabaseContext, DevelopeService developeService)
        {
            _recipeDatabaseContext = recipeDatabaseContext;
            _developeService = developeService;
        }

        private void JsonSerializeForProduction()
        {
            var alergens = _recipeDatabaseContext.Alergens.Include(a => a.IngredientAlergens).ToList();

            using (var sr = new StreamWriter("Alergen.json"))
            {
                sr.Write(JsonSerializer.Serialize(alergens));
            }

            var addinfo = _recipeDatabaseContext.AdditionalInfos.ToList();

            using (var sr = new StreamWriter("Additionalinfo.json"))
            {
                sr.Write(JsonSerializer.Serialize(addinfo));
            }

            var cuisine = _recipeDatabaseContext.CuisineTypes.ToList();

            using (var sr = new StreamWriter("CuisineType.json"))
            {
                sr.Write(JsonSerializer.Serialize(cuisine));
            }

            var dietmeal = _recipeDatabaseContext.DietMeals.ToList();

            using (var sr = new StreamWriter("DietMeal.json"))
            {
                sr.Write(JsonSerializer.Serialize(dietmeal));
            }

            var diettable = _recipeDatabaseContext.DietTables.ToList();

            using (var sr = new StreamWriter("DietTable.json"))
            {
                sr.Write(JsonSerializer.Serialize(diettable));
            }

            var ingralerg = _recipeDatabaseContext.IngredientAlergens.ToList();

            using (var sr = new StreamWriter("IngredientAlergen.json"))
            {
                sr.Write(JsonSerializer.Serialize(ingralerg));
            }

            var ingrprice = _recipeDatabaseContext.IngredientsPrices.ToList();

            using (var sr = new StreamWriter("IngredientsPrice.json"))
            {
                sr.Write(JsonSerializer.Serialize(ingrprice));
            }

            var ingrtable = _recipeDatabaseContext.IngredientsTables.ToList();

            using (var sr = new StreamWriter("IngredientsTable.json"))
            {
                sr.Write(JsonSerializer.Serialize(ingrtable));
            }

            var kindofmeal = _recipeDatabaseContext.KindOfMeals.ToList();

            using (var sr = new StreamWriter("KindOfMeal.json"))
            {
                sr.Write(JsonSerializer.Serialize(kindofmeal));
            }

            var maintable = _recipeDatabaseContext.MainTables.ToList();

            using (var sr = new StreamWriter("MainTable.json"))
            {
                sr.Write(JsonSerializer.Serialize(maintable));
            }

            var units = _recipeDatabaseContext.Units.ToList();

            using (var sr = new StreamWriter("Unit.json"))
            {
                sr.Write(JsonSerializer.Serialize(units));
            }
        }

        private void JsonDeserializeForProduction()
        {         
            using (var sr = new StreamReader("Additionalinfo.json"))
            {
                addinfo = JsonSerializer.Deserialize<List<AdditionalInfo>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("CuisineType.json"))
            {
                cuisineTypes = JsonSerializer.Deserialize<List<CuisineType>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("DietMeal.json"))
            {
                dietMeals = JsonSerializer.Deserialize<List<DietMeal>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("DietTable.json"))
            {
                dietTables = JsonSerializer.Deserialize<List<DietTable>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("IngredientsTable.json"))
            {
                ingredientsTables = JsonSerializer.Deserialize<List<IngredientsTable>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("MealIngreadients.json"))
            {
                mealIngredients = JsonSerializer.Deserialize<List<MealIngredient>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("KindOfMeal.json"))
            {
                kindOfMeals = JsonSerializer.Deserialize<List<KindOfMeal>>(sr.ReadToEnd())!;
            }

            using (var sr = new StreamReader("MainTable.json"))
            {
                maintables = JsonSerializer.Deserialize<List<MainTable>>(sr.ReadToEnd())!;
            }

        }

        [HttpGet]
        [Route("/GetAllRecipies")]
        public List<Recipe> GetAllRecipies(int? count)
        {
            List<Recipe> Recipies = new List<Recipe>();
            List<MainTable> mainTableList = null!;

            if (!_developeService.isProduction)
            {              
                if(count != null)
                {
                    mainTableList = _recipeDatabaseContext.MainTables.Take((int)count).ToList();
                }
                else
                {
                    mainTableList = _recipeDatabaseContext.MainTables.ToList();
                }
             
                foreach(var rec in mainTableList)
                {            
                    var info = _recipeDatabaseContext.AdditionalInfos
                        .Include(a => a.IdCuisineNavigation)
                        .Include(a => a.IdKindOfMealNavigation)
                        .Where(a => a.IdMeal.Equals(rec.Id)).FirstOrDefault();

                    var diet = _recipeDatabaseContext.DietMeals.Include(d => d.IdDietNavigation).Where(d => d.IdMeal.Equals(rec.Id)).FirstOrDefault();

                    try
                    {
                        var recipe = new Recipe()
                        {
                            Id = rec.Id,
                            Description = rec?.Description,
                            Name = rec?.Name,
                            Calories = rec?.Calories,
                            CookingTime = info?.CookingTime,
                            Image = info?.Image,
                            CuisineType = info?.IdCuisineNavigation?.Name,
                            KindOfMeal = info?.IdKindOfMealNavigation?.Name,
                            Diet = diet?.IdDietNavigation?.Name,
                            Ingredients = _recipeDatabaseContext.MealIngredients
                                      .Include(m => m.IdIngredientNavigation)
                                      .Where(m => m.IdMeal.Equals(rec.Id))
                                      .ToDictionary(r => r.IdIngredientNavigation.Name, r => r.Quantity)
                        };

                        recipe.IngredientCount = recipe.Ingredients.Count();

                        Recipies.Add(recipe);
                    }
                    catch(Exception ex)
                    {

                    }               
                }
            }
            else
            {
                JsonDeserializeForProduction();

                if (count != null)
                {
                    mainTableList = maintables.Take((int)count).ToList();
                }
                else
                {
                    mainTableList = maintables.ToList();
                }
           
                foreach (var rec in mainTableList)
                {
                    var info = addinfo.Where(a => a.IdMeal.Equals(rec.Id)).FirstOrDefault();
                    info!.IdCuisineNavigation = cuisineTypes.Where(c => c.Id.Equals(info.IdCuisine)).FirstOrDefault();
                    info.IdKindOfMealNavigation = kindOfMeals.Where(k => k.Id.Equals(info.IdKindOfMeal)).FirstOrDefault()!;

                    var diet = dietMeals.Where(d => d.IdMeal.Equals(rec.Id)).FirstOrDefault();

                    if(diet != null)
                    {
                        diet.IdDietNavigation = dietTables.Where(d => d.Id.Equals(diet.IdDiet)).FirstOrDefault()!;
                    }
                    
                    var ingr = mealIngredients.Where(a => a.IdMeal.Equals(rec.Id)).ToList();
                    
                    foreach(var i in ingr)
                    {
                        i.IdIngredientNavigation = ingredientsTables.Where(t => t.Id.Equals(i.IdIngredient)).FirstOrDefault()!;
                    }

                    try
                    {
                        var recipe = new Recipe()
                        {
                            Id = rec.Id,
                            Description = rec?.Description,
                            Name = rec?.Name,
                            Calories = rec?.Calories,
                            CookingTime = info?.CookingTime,
                            Image = info?.Image,
                            CuisineType = info?.IdCuisineNavigation?.Name,
                            KindOfMeal = info?.IdKindOfMealNavigation?.Name,
                            Diet = diet?.IdDietNavigation?.Name,
                            Ingredients = ingr.ToDictionary(r => r.IdIngredientNavigation.Name, r => r.Quantity)

                        };

                        recipe.IngredientCount = recipe.Ingredients.Count();

                        Recipies.Add(recipe);
                    }
                    catch(Exception ex)
                    {

                    }
           
                }
            }

            return Recipies;
        }

        private Recipe CreateRecipeForLocal(MainTable rec)
        {
            var info = _recipeDatabaseContext.AdditionalInfos
                        .Include(a => a.IdCuisineNavigation)
                        .Include(a => a.IdKindOfMealNavigation)
                        .Where(a => a.IdMeal.Equals(rec.Id)).FirstOrDefault();

            var diet = _recipeDatabaseContext.DietMeals.Include(d => d.IdDietNavigation).Where(d => d.IdMeal.Equals(rec.Id)).FirstOrDefault();

            try
            {
                var recipe = new Recipe()
                {
                    Id = rec.Id,
                    Description = rec?.Description,
                    Name = rec?.Name,
                    Calories = rec?.Calories,
                    CookingTime = info?.CookingTime,
                    Image = info?.Image,
                    CuisineType = info?.IdCuisineNavigation?.Name,
                    KindOfMeal = info?.IdKindOfMealNavigation?.Name,
                    Diet = diet?.IdDietNavigation?.Name,
                    Ingredients = _recipeDatabaseContext.MealIngredients
                              .Include(m => m.IdIngredientNavigation)
                              .Where(m => m.IdMeal.Equals(rec.Id))
                              .ToDictionary(r => r.IdIngredientNavigation.Name, r => r.Quantity)
                };

                recipe.IngredientCount = recipe.Ingredients.Count();            

                return recipe;
            }
            catch (Exception ex)
            {
                return null!;
            }
         
        }

        private Recipe CreateRecipeForProduction(MainTable rec)
        {
            var info = addinfo.Where(a => a.IdMeal.Equals(rec.Id)).FirstOrDefault();
            info!.IdCuisineNavigation = cuisineTypes.Where(c => c.Id.Equals(info.IdCuisine)).FirstOrDefault();
            info.IdKindOfMealNavigation = kindOfMeals.Where(k => k.Id.Equals(info.IdKindOfMeal)).FirstOrDefault()!;

            var diet = dietMeals.Where(d => d.IdMeal.Equals(rec.Id)).FirstOrDefault();

            if (diet != null)
            {
                diet.IdDietNavigation = dietTables.Where(d => d.Id.Equals(diet.IdDiet)).FirstOrDefault()!;
            }

            var ingr = mealIngredients.Where(a => a.IdMeal.Equals(rec.Id)).ToList();

            foreach (var i in ingr)
            {
                i.IdIngredientNavigation = ingredientsTables.Where(t => t.Id.Equals(i.IdIngredient)).FirstOrDefault()!;
            }

            try
            {
                var recipe = new Recipe()
                {
                    Id = rec.Id,
                    Description = rec?.Description,
                    Name = rec?.Name,
                    Calories = rec?.Calories,
                    CookingTime = info?.CookingTime,
                    Image = info?.Image,
                    CuisineType = info?.IdCuisineNavigation?.Name,
                    KindOfMeal = info?.IdKindOfMealNavigation?.Name,
                    Diet = diet?.IdDietNavigation?.Name,
                    Ingredients = ingr.ToDictionary(r => r.IdIngredientNavigation.Name, r => r.Quantity)
                };

                recipe.IngredientCount = recipe.Ingredients.Count();

                return recipe;
            }
            catch (Exception ex)
            {
                return null!;
            }

        }

        private RecipeWithPattern RemoveDupliateMeals(RecipeWithPattern rwp)
        {
            List<Recipe> recipeForRemove = new List<Recipe>();

            foreach (var r in rwp.RecipesWithPatternInName)
            {
                var recipe = rwp.RecipesWithPatternInIngredient.Where(rc => rc.Id.Equals(r.Id)).FirstOrDefault();

                if(recipe != null)
                {
                    rwp.RecipesWithPatternInIngredient.Remove(recipe);
                }
            }
          
            return rwp;
        }   
            

        [HttpGet]
        [Route("/GetRecipiesByNameOrIngredient")]
        public RecipeWithPattern GetRecipiesByPattern(string pattern)
        {
            if(string.IsNullOrEmpty(pattern))
            {
                return null!;
            }

            RecipeWithPattern RecipiesWithPattern = new RecipeWithPattern();
            List<MainTable> mainTableList = null!;

            if (!_developeService.isProduction)
            {
                mainTableList = _recipeDatabaseContext.MainTables.ToList();

                mainTableList = mainTableList.Where(m => m.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var rec in mainTableList)
                {
                    var recipe = CreateRecipeForLocal(rec);

                    if (recipe != null)
                    {
                        var check = recipe.Ingredients?.Where(i => i.Key.Contains(pattern)).ToDictionary(d => d.Key, d => d.Value);

                        if (check?.Count != 0)
                             RecipiesWithPattern.RecipesWithPatternInName.Add(recipe);
                    }
                }

                var ingredietns = _recipeDatabaseContext.IngredientsTables.ToList();

                ingredietns = ingredietns.Where(i => i.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

                List<MealIngredient> mealingredients = new List<MealIngredient>();

                foreach(var i in ingredietns)
                {
                    var mealingredient = _recipeDatabaseContext.MealIngredients.Where(m => m.IdIngredient.Equals(i.Id)).FirstOrDefault();

                    if(mealingredient != null)
                    {
                        mealingredients.Add(mealingredient);
                    }          
                }

                List<MainTable> mainTableListForIngredientsPattern = new List<MainTable>();

                mealingredients = mealingredients.DistinctBy(m => m.Id).ToList();

                foreach(var meal in mealingredients)
                {
                    var mealingredientpattern = _recipeDatabaseContext.MainTables.Where(m => m.Id.Equals(meal.IdMeal)).FirstOrDefault();

                    if(mealingredientpattern != null)
                    {
                        var recipe = CreateRecipeForLocal(mealingredientpattern);

                        if(recipe != null)
                        {               
                             RecipiesWithPattern.RecipesWithPatternInIngredient.Add(recipe);
                        }
                    }
                    
                }

                RecipiesWithPattern.RecipesWithPatternInIngredient = RecipiesWithPattern.RecipesWithPatternInIngredient.DistinctBy(r => r.Id).ToList();
                RecipiesWithPattern.RecipesWithPatternInName = RecipiesWithPattern.RecipesWithPatternInName.DistinctBy(r => r.Id).ToList();

                RecipiesWithPattern = RemoveDupliateMeals(RecipiesWithPattern);

                RecipiesWithPattern.Count = RecipiesWithPattern.RecipesWithPatternInIngredient.Count + RecipiesWithPattern.RecipesWithPatternInName.Count;
            }
            else
            {
                JsonDeserializeForProduction();

                mainTableList = maintables.ToList();

                mainTableList = mainTableList.Where(m => m.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var rec in mainTableList)
                {
                    var recipe = CreateRecipeForProduction(rec);

                    if (recipe != null)
                    {
                        var check = recipe.Ingredients?.Where(i => i.Key.Contains(pattern)).ToDictionary(d => d.Key, d => d.Value);

                        if (check?.Count != 0)
                            RecipiesWithPattern.RecipesWithPatternInName.Add(recipe);
                    }
                }

                var ingredietns = _recipeDatabaseContext.IngredientsTables.ToList();

                ingredietns = ingredietns.Where(i => i.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

                List<MealIngredient> mealingredients = new List<MealIngredient>();

                foreach (var i in ingredietns)
                {
                    var mealingredient = _recipeDatabaseContext.MealIngredients.Where(m => m.IdIngredient.Equals(i.Id)).FirstOrDefault();

                    if (mealingredient != null)
                    {
                        mealingredients.Add(mealingredient);
                    }
                }

                List<MainTable> mainTableListForIngredientsPattern = new List<MainTable>();

                mealingredients = mealingredients.DistinctBy(m => m.Id).ToList();

                foreach (var meal in mealingredients)
                {
                    var mealingredientpattern = _recipeDatabaseContext.MainTables.Where(m => m.Id.Equals(meal.IdMeal)).FirstOrDefault();

                    if (mealingredientpattern != null)
                    {
                        var recipe = CreateRecipeForProduction(mealingredientpattern);

                        if (recipe != null)
                        {
                            RecipiesWithPattern.RecipesWithPatternInIngredient.Add(recipe);
                        }
                    }

                }

                RecipiesWithPattern.RecipesWithPatternInIngredient = RecipiesWithPattern.RecipesWithPatternInIngredient.DistinctBy(r => r.Id).ToList();
                RecipiesWithPattern.RecipesWithPatternInName = RecipiesWithPattern.RecipesWithPatternInName.DistinctBy(r => r.Id).ToList();

                RecipiesWithPattern = RemoveDupliateMeals(RecipiesWithPattern);

                RecipiesWithPattern.Count = RecipiesWithPattern.RecipesWithPatternInIngredient.Count + RecipiesWithPattern.RecipesWithPatternInName.Count;
            }
           

            return RecipiesWithPattern;
        }
    }
}