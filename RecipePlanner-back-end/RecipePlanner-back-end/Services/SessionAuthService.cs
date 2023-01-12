using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Models.Users;
using RecipePlanner_back_end.Services;

namespace RecipePlanner_back_end.Services
{
    public class SessionAuthService : IAuthService
    {
        private readonly UserdbContext _userdbContext;
        public User User { get; set; }

        public SessionAuthService(UserdbContext context)
        {
            _userdbContext = context;
            User = null!;
        }

        public void Set(string id)
        {
            User = _userdbContext.Users.Find(Guid.Parse(id))!;
        }
    }
}
