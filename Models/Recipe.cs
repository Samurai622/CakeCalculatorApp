using System;
using System.Collections.Generic;

namespace CakeCalculatorApp.Models
{
    public class Recipe
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;    

        public Shape? CakeShape { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}