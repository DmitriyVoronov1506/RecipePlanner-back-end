using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Models;
using RecipePlanner_back_end.Models.Users;
using RecipePlanner_back_end.Services;

namespace RecipePlanner_back_end.Controllers
{
    [Route("AuthController")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHasher _hasher;
        private readonly UserdbContext _userdbContext;
        private readonly IAuthService _authService;

        public AuthController(IHasher hasher, UserdbContext userdbContext, IAuthService authService)
        {
            _hasher = hasher;
            _userdbContext = userdbContext;
            _authService = authService;
        }

        [HttpGet]
        [Route("GetCurrentUser")]
        public User GetCurrentUser()
        {
            string userid = Request.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new Models.Users.User();
            }

            return _userdbContext.Users.Find(Guid.Parse(userid))! with { PassHash = "*", PassSalt = "*" };
        }

        [HttpPost]
        [Route("Register")]
        public JsonResult Register([FromForm] RegUserModel RegUser)
        {
            if (String.IsNullOrEmpty(RegUser.UserName) || RegUser.UserName.Length < 3)
            {
                return new JsonResult("User name cant be empty or less then 3 letters!");
            }

            if(String.IsNullOrEmpty(RegUser.Email))
            {
                return new JsonResult("Email cant be empty!");
            }

            if(String.IsNullOrEmpty(RegUser.Gender))
            {
                return new JsonResult("You have to choose gender!");
            }

            if (RegUser.BirthdayDate == null)
            {
                return new JsonResult("You have to choose birthday date!");
            }

            if (RegUser.Region == "Choose region")
            {
                return new JsonResult("You have to choose region!");
            }

            if (String.IsNullOrEmpty(RegUser.Password1) || String.IsNullOrEmpty(RegUser.Password2) || RegUser.Password1.Length < 5)
            {
                return new JsonResult("Password cant be empty or less then 5 symbols!");
            }

            var userWithEmail = _userdbContext.Users.Where(u => u.Email == RegUser.Email).FirstOrDefault();

            if(userWithEmail != null)
            {
                return new JsonResult("User with this email already exists!");
            }

            if(!RegUser.Password1.Equals(RegUser.Password2))
            {
                return new JsonResult("Passwords must be similar!");
            }

            var user = new User();

            user.Email = RegUser.Email;
            user.PassSalt = _hasher.Hash(DateTime.Now.ToString());
            user.PassHash = _hasher.Hash(RegUser.Password1 + user.PassSalt);
            user.UserName = RegUser.UserName;
            user.Gender = RegUser.Gender;
            user.BirthdayDate = RegUser.BirthdayDate;
            user.Region = RegUser.Region;
            user.RegMoment = DateTime.Now;
     
            _userdbContext.Users.Add(user);
            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }

        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromForm] string email, [FromForm] string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult("Login required");
            }

            if (string.IsNullOrEmpty(password))
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult("Password required");
            }

            var user = _userdbContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if (user == null)
            {
                HttpContext.Response.StatusCode = 401;
                return new JsonResult("Wrong email!");
            }

            string PassHash = _hasher.Hash(password + user.PassSalt);

            if (PassHash != user.PassHash)
            {
                HttpContext.Response.StatusCode = 401;
                return new JsonResult("Credentials invalid! Wrong password!");
            }

            Response.Headers.Add("Access-Control-Expose-Headers", "*");
            Response.Headers.Add("userid", user.Id.ToString());

            return new JsonResult("Ok");
        }

        [HttpPost]
        [Route("LogOut")]
        public string LogOut()
        {
            if (_authService.User != null)
            {
                HttpContext.Session.Remove("userId");
                return "Ok";
            }

            return "Ok";
        }

        [HttpPut]
        [Route("ChangePassword")]
        public string ChangePassword(string oldPassword, string newPassword1, string newPassword2)
        {
            if(_authService.User == null)
            {
                return "You need to log in first!";
            }

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword1) || string.IsNullOrEmpty(newPassword2) || newPassword1.Length < 5)
            {
                return "Password cant be empty or less then 5 symbols!";
            }

            if(!newPassword1.Equals(newPassword2))
            {
                return "Passwords must be similar!";
            }

            string OldPassHash = _hasher.Hash(oldPassword + _authService.User.PassSalt);
            string NewPassHash = _hasher.Hash(newPassword1 + _authService.User.PassSalt);

            if (OldPassHash.Equals(NewPassHash))
            {
                return "old and new passwords cant be similar!";
            }

            _authService.User.PassHash = NewPassHash;

            _userdbContext.SaveChanges();

            return "Ok";
        }
    }
}
