using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class IngredientsPrice
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? Quantity { get; set; }
    }
}
