using System.Collections.Generic;
using System.Threading.Tasks;
using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Data
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task SaveRecipeAsync(Recipe recipe);
    }
}