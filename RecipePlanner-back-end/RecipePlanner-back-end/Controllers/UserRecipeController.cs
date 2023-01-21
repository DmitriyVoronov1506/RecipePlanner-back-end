using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Entities;
using RecipePlanner_back_end.Models.Users;

namespace RecipePlanner_back_end.Controllers
{
    [Route("RecipesFilterController")]
    [ApiController]
    public class UserRecipeController : ControllerBase
    {
        private readonly UserdbContext _userdbContext;

        public UserRecipeController(UserdbContext userdbContext)
        {
            _userdbContext = userdbContext;
        }

        [HttpGet]
        [Route("GetUsersRecipies")]
        public JsonResult GetUsersRecipies()
        {
            string userid = Request.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            var resultRecipies = _userdbContext.UsersRecipies.Where(u => u.IdUser.Equals(Guid.Parse(userid))).ToList();

            return new JsonResult(resultRecipies);
        }

        [HttpPost]
        [Route("AddNewRecipe")]
        public JsonResult AddNewRecipe([FromForm] UserRecipe userRecipy)
        {
            string userid = Request.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            Guid usersId;

            try
            {
                usersId = Guid.Parse(userid);
            }
            catch(Exception ex)
            {
                return new JsonResult("Wrong user data sent to server! Please relogin!");
            }

            if(string.IsNullOrEmpty(userRecipy.Name))
            {
                return new JsonResult("Recipe name cant be empty!");
            }

            if (string.IsNullOrEmpty(userRecipy.Ingredients))
            {
                return new JsonResult("Recipe ingredients cant be empty!");
            }

            if (string.IsNullOrEmpty(userRecipy.Description))
            {
                return new JsonResult("Recipe description cant be empty!");
            }

            var resultRecipies = _userdbContext.UsersRecipies.Where(u => u.IdUser.Equals(usersId) && u.Name == userRecipy.Name).FirstOrDefault();

            if(resultRecipies != null)
            {
                return new JsonResult("Recipe with this name already exists!");
            }

            UsersRecipy usersRecipy = new UsersRecipy()
            {
                IdUser = usersId,
                Name = userRecipy.Name,
                Description = userRecipy.Description,
                Calories = userRecipy.Name,
                CookingTime = userRecipy.CookingTime,
                Image = userRecipy.Image,
                CuisineType = userRecipy.CuisineType,
                KindOfMeal = userRecipy.KindOfMeal,
                Diet = userRecipy.Diet,
                Ingredients = userRecipy.Ingredients,
                IngredientCount = userRecipy.IngredientCount,
                AddingDate = DateTime.Now
            };

            _userdbContext.UsersRecipies.Add(usersRecipy);
            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }

    }
}
