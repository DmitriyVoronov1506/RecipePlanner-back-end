using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("Register")]
        public string Register([FromBody] RegUserModel RegUser)
        {
            if (String.IsNullOrEmpty(RegUser.UserName) || RegUser.UserName.Length < 3)
            {
                return "User name cant be empty or less then 3 letters!";
            }

            if(String.IsNullOrEmpty(RegUser.Email))
            {
                return "Email cant be empty!";
            }

            if (String.IsNullOrEmpty(RegUser.Password1) || String.IsNullOrEmpty(RegUser.Password2) || RegUser.Password1.Length < 5)
            {
                return "Password cant be empty or less then 5 symbols!";
            }

            var userWithEmail = _userdbContext.Users.Where(u => u.Email == RegUser.Email).FirstOrDefault();

            if(userWithEmail != null)
            {
                return "User with this email alresy exists!";
            }

            if(!RegUser.Password1.Equals(RegUser.Password2))
            {
                return "Passwords must be similar!";
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

            return "Ok";
        }

        [HttpGet]
        [Route("Login")]
        public string Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                HttpContext.Response.StatusCode = 409;
                return "Login required";
            }

            if (string.IsNullOrEmpty(password))
            {
                HttpContext.Response.StatusCode = 409;
                return "Password required";
            }

            var user = _userdbContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if (user == null)
            {
                HttpContext.Response.StatusCode = 401;
                return "Wrong email!";
            }

            string PassHash = _hasher.Hash(password + user.PassSalt);

            if (PassHash != user.PassHash)
            {
                HttpContext.Response.StatusCode = 401;
                return "Credentials invalid! Wrong password!";
            }

            HttpContext.Session.SetString("userId", user.Id.ToString());

            HttpContext.Session.SetString("AuthMoment", DateTime.Now.Ticks.ToString());

            return "Ok";
        }

        [HttpGet]
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
