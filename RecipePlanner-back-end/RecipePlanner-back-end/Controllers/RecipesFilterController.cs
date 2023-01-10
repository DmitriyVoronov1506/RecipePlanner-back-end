using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Entities;
using RecipePlanner_back_end.Models.Recipes;
using RecipePlanner_back_end.Services;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace RecipePlanner_back_end.Controllers
{
    [ApiController]
    [Route("RecipesFilterController")]
    public class RecipesFilterController : ControllerBase
    {
        private readonly RecipeDatabaseContext _recipeDatabaseContext;
        private readonly UserdbContext _userdbContext;
        private readonly DevelopeService _developeService;

        public RecipesFilterController(RecipeDatabaseContext recipeDatabaseContext, DevelopeService developeService, UserdbContext userdbContext)
        {
            _recipeDatabaseContext = recipeDatabaseContext;
            _developeService = developeService;
            _userdbContext = userdbContext;
        }


        [HttpGet]
        [Route("GetAllRecipies")]
        public List<Recipe> GetAllRecipies(int count)
        {
            List<Recipe> Recipies = new List<Recipe>();
            List<MainTable> mainTableList = null!;

            int limit = 10;
            int skip = 0;

            mainTableList = _recipeDatabaseContext.MainTables.ToList();

            if(count == 0)
            {
                return null!;
            }

            if (count * limit - limit < mainTableList.Count)
            {
                skip += limit * count - limit;

                mainTableList = mainTableList.Skip(skip).Take(limit).ToList();
            }
            else
            {
                return null!;
            }
     
            foreach (var rec in mainTableList)
            {
                var recipe = CreateRecipeForLocal(rec);

                if(recipe != null)
                {
                    Recipies.Add(recipe);
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
                    Ingredients = (_recipeDatabaseContext.MealIngredients
                                  .Include(m => m.IdIngredientNavigation)
                                  .Where(m => m.IdMeal.Equals(rec.Id)).ToList()).DistinctBy(m => m.IdIngredientNavigation.Name)
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
        [Route("GetRecipiesByNameOrIngredient")]                              
        public RecipeWithPattern GetRecipiesByPattern(string? pattern)
        {
            if(string.IsNullOrEmpty(pattern))
            {
                return null!;
            }

            RecipeWithPattern RecipiesWithPattern = new RecipeWithPattern();
            List<MainTable> mainTableList = null!;

            Regex regex = new Regex(@$"^(.+\s+{pattern}([s]*|[es]*)\s+.+)|(.+\s+{pattern}([s]*|[es]*)$)|(^{pattern}([s]*|[es]*)$)|(^{pattern}([s]*|[es]*)\s+.*)$", RegexOptions.IgnoreCase);

            mainTableList = _recipeDatabaseContext.MainTables.ToList();

            mainTableList = mainTableList.Where(m => regex.IsMatch(m.Name)).ToList();

            foreach (var rec in mainTableList)
            {
                var recipe = CreateRecipeForLocal(rec);

                if (recipe != null)
                {
                    RecipiesWithPattern.RecipesWithPatternInName.Add(recipe);
                }
            }

            var ingredietns = _recipeDatabaseContext.IngredientsTables.ToList();

            ingredietns = ingredietns.Where(i => regex.IsMatch(i.Name)).ToList();

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
                    var recipe = CreateRecipeForLocal(mealingredientpattern);

                    if (recipe != null)
                    {
                        RecipiesWithPattern.RecipesWithPatternInIngredient.Add(recipe);
                    }
                }

            }

            RecipiesWithPattern.RecipesWithPatternInIngredient = RecipiesWithPattern.RecipesWithPatternInIngredient.DistinctBy(r => r.Id).ToList();
            RecipiesWithPattern.RecipesWithPatternInName = RecipiesWithPattern.RecipesWithPatternInName.DistinctBy(r => r.Id).ToList();

            RecipiesWithPattern = RemoveDupliateMeals(RecipiesWithPattern);

            RecipiesWithPattern.CreatePaggination();

            return RecipiesWithPattern;
        }


        [HttpPost]
        [Route("DontCallMeFromFrontEnd")]
        public void CreateUsersRecipy()
        {
            //_userdbContext.UsersRecipies.Add(new UsersRecipy() { IdUser = Guid.NewGuid(), Diet = "1", Name = "123", IngredientCount = 10, AddingDate = DateTime.Now });
            //_userdbContext.SaveChanges();

            //var meals = _recipeDatabaseContext.MainTables.Where(t => t.Name.Contains("Ukrainian")).ToList();

            //meals.ForEach(m => m.Name = m.Name.Replace("Russian", "Ukrainian"));


            //_recipeDatabaseContext.SaveChanges();
        }
    }
}