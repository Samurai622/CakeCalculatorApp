using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Services
{
    /// <summary>
    /// Інтерфейс сервісу калькуляції. Інкапсулює бізнес-логіку перерахунку рецептів.
    /// </summary>
    public interface ICakeCalculatorService
    {
        /// <summary>
        /// Обчислює нові пропорції інгредієнтів для цільового торта на основі оригінального рецепта.
        /// </summary>
        /// <param name="originalRecipe">Об'єкт оригінального рецепта з базовими інгредієнтами.</param>
        /// <param name="targetShape">Геометрична форма та розміри цільового торта.</param>
        /// <param name="targetLayers">Кількість коржів цільового торта.</param>
        /// <param name="excludeSurface">Прапорець: чи потрібно виключити з розрахунку поверхневі інгредієнти (глазур).</param>
        /// <returns>Новий об'єкт рецепта з перерахованими масами інгредієнтів.</returns>
        Recipe CalculateNewRecipe(Recipe originalRecipe, Shape targetShape, int targetLayers, bool excludeSurface);
    }
}