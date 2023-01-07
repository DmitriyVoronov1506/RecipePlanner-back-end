using System;
using System.Collections.Generic;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
