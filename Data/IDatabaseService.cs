using System.Collections.Generic;
using System.Threading.Tasks;
using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Data
{
    /// <summary>
    /// Абстрактний інтерфейс для рівня доступу до даних (Data Access Layer).
    /// Забезпечує дотримання принципу інверсії залежностей (D з SOLID).
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Асинхронно отримує список усіх доступних рецептів з бази даних.
        /// </summary>
        /// <returns>Колекція об'єктів Recipe.</returns>
        Task<IEnumerable<Recipe>> GetRecipesAsync();

        /// <summary>
        /// Асинхронно зберігає або оновлює переданий рецепт у базі даних.
        /// </summary>
        /// <param name="recipe">Рецепт, який необхідно зберегти.</param>
        Task SaveRecipeAsync(Recipe recipe);
    }
}