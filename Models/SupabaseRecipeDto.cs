using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace CakeCalculatorApp.Models
{
    [Table("cloud_recipes")]
    public class SupabaseRecipeDto : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("recipe_json")]
        public string RecipeJson { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}