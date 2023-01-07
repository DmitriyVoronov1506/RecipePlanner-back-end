using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipePlanner_back_end.Models.Recipes
{
    public partial class CuisineType
    {
        public CuisineType()
        {
            AdditionalInfos = new HashSet<AdditionalInfo>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; }
    }
}
