using RecipePlanner_back_end.Models.Users;

namespace RecipePlanner_back_end.Services
{
    public interface IAuthService
    {
        public User User { get; set; }

        public void Set(string id);
    }
}
