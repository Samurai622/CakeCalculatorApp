using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Services
{
    public interface ICakeCalculatorService
    {
        Recipe CalculateNewRecipe(Recipe originalRecipe, Shape targetShape, int targetLayers, bool excludeSurface);
    }
}