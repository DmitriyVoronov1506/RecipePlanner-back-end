using Microsoft.AspNetCore.Http;

namespace RecipePlanner_back_end.Models
{
    public class RegUserModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password1 { get; set; }
        public string? Password2 { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string? Gender { get; set; }
        public string? Region { get; set; }
    }
}
