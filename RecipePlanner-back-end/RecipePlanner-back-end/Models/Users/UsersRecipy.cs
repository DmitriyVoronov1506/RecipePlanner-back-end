namespace RecipePlanner_back_end.Models.Users
{
    public class UsersRecipy
    {
        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Calories { get; set; }
        public string? CookingTime { get; set; }
        public string? Image { get; set; }
        public string? CuisineType { get; set; }
        public string? KindOfMeal { get; set; }
        public string? Diet { get; set; }
        public string? Ingredients { get; set; }
        public int? IngredientCount { get; set; }
        public DateTime? AddingDate { get; set; }
    }
}
