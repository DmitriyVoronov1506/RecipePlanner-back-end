using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Users
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PassHash { get; set; }
        public string? PassSalt { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string? Gender { get; set; }
        public string? Region { get; set; }
        public DateTime? RegMoment { get; set; }
    }
}
