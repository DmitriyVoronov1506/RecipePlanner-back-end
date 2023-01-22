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

        public AuthController(IHasher hasher, UserdbContext userdbContext)
        {
            _hasher = hasher;
            _userdbContext = userdbContext;
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
                return new JsonResult("Login required");
            }

            if (string.IsNullOrEmpty(password))
            {
                return new JsonResult("Password required");
            }

            var user = _userdbContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if (user == null)
            {
                return new JsonResult("Wrong email!");
            }

            string PassHash = _hasher.Hash(password + user.PassSalt);

            if (PassHash != user.PassHash)
            {
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
            return "Ok";
        }

        [HttpPut]
        [Route("ChangePassword")]
        public JsonResult ChangePassword(string? oldPassword, string? newPassword1, string? newPassword2)
        {
            string userid = Request.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword1) || string.IsNullOrEmpty(newPassword2) || newPassword1.Length < 5)
            {
                return new JsonResult("Password cant be empty or less then 5 symbols!");
            }

            if(!newPassword1.Equals(newPassword2))
            {
                return new JsonResult("Passwords must be similar!");
            }

            var user = _userdbContext.Users.Find(Guid.Parse(userid));

            string OldPassHash = _hasher.Hash(oldPassword + user.PassSalt);

            if(!OldPassHash.Equals(user.PassHash))
            {
                return new JsonResult("You wrote a wrong password!");
            }

            string NewPassHash = _hasher.Hash(newPassword1 + user.PassSalt);

            if (OldPassHash.Equals(NewPassHash))
            {
                return new JsonResult("Old and new password cant be similar!");
            }

            user.PassHash = NewPassHash;

            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }

        [HttpPut]
        [Route("ChangeUserName")]
        public JsonResult ChangeUserName(string? newName)
        {
            string? userid = Request?.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            if(string.IsNullOrEmpty(newName) || newName.Length < 3)
            {
                return new JsonResult("User name cant be empty or less then 3 letters!");
            }

            var user = _userdbContext.Users.Find(Guid.Parse(userid));
            user.UserName = newName;

            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }

        [HttpPut]
        [Route("ChangeEmail")]
        public JsonResult ChangeEmail(string? newEmail)
        {
            string? userid = Request?.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            if (string.IsNullOrEmpty(newEmail))
            {
                return new JsonResult("Email cant be empty!");
            }

            var userWithEmail = _userdbContext.Users.Where(u => u.Email == newEmail).FirstOrDefault();

            if (userWithEmail != null)
            {
                return new JsonResult("User with this email already exists!");
            }

            var user = _userdbContext.Users.Find(Guid.Parse(userid));
            user.Email = newEmail;

            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }

        [HttpPut]
        [Route("ChangeBirthdayDate")]
        public JsonResult ChangeBirthdayDate(DateTime? newBirthday)
        {
            string? userid = Request?.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            if (newBirthday == null)
            {
                return new JsonResult("Choose new birthday date!");
            }

            var user = _userdbContext.Users.Find(Guid.Parse(userid));
            user.BirthdayDate = newBirthday;

            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }

        [HttpPut]
        [Route("ChangeRegion")]
        public JsonResult ChangeRegion(string? newRegion)
        {
            string? userid = Request?.Headers["current-user-id"];

            if (String.IsNullOrEmpty(userid))
            {
                return new JsonResult("Unauthorized");
            }

            if (newRegion == "Choose region")
            {
                return new JsonResult("You have to choose region!");
            }

            var user = _userdbContext.Users.Find(Guid.Parse(userid));
            user.Region = newRegion;

            _userdbContext.SaveChanges();

            return new JsonResult("Ok");
        }
    }
}
