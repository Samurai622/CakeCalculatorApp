using System;
using System.Linq;
using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Services
{
    /// <summary>
    /// Сервіс, що містить математичні алгоритми обчислення коефіцієнтів 
    /// масштабування (за об'ємом, площею та кількістю прошарків крему).
    /// </summary>
    public class CakeCalculatorService : ICakeCalculatorService
    {
        public Recipe CalculateNewRecipe(Recipe originalRecipe, Shape targetShape, int targetLayers, bool excludeSurface)
        {
            if (originalRecipe == null || originalRecipe.CakeShape == null) throw new ArgumentException("Original recipe cannot be null.");
            if (targetShape == null) throw new ArgumentException("Target shape cannot be null.");

            double originalVolume = originalRecipe.CakeShape.GetVolume();
            double originalArea = originalRecipe.CakeShape.GetSurfaceArea();
            
            int oldCreamGaps = Math.Max(1, originalRecipe.Layers - 1);
            int newCreamGaps = Math.Max(1, targetLayers - 1);

            double originalHeight = originalRecipe.CakeShape is CylinderShape cs1 ? cs1.Height : 
                                   (originalRecipe.CakeShape is CuboidShape cb1 ? cb1.Height : 10);
            
            double targetHeight = targetShape is CylinderShape cs2 ? cs2.Height : 
                                 (targetShape is CuboidShape cb2 ? cb2.Height : 10);

            double originalLayerThickness = originalHeight / Math.Max(1, originalRecipe.Layers);
            double targetLayerThickness = targetHeight / Math.Max(1, targetLayers);

            double originalCreamVolume = originalRecipe.CakeShape.GetBaseArea() * oldCreamGaps * originalLayerThickness;
            double targetCreamVolume = targetShape.GetBaseArea() * newCreamGaps * targetLayerThickness;

            if (originalVolume <= 0 || originalArea <= 0 || originalCreamVolume <= 0)
                throw new InvalidOperationException("Cannot calculate ratios with zero values.");

            double volumeRatio = targetShape.GetVolume() / originalVolume;
            double areaRatio = targetShape.GetSurfaceArea() / originalArea;
            double creamRatio = targetCreamVolume / originalCreamVolume;

            var newRecipe = new Recipe
            {
                Name = $"{originalRecipe.Name} (Перераховано)",
                CakeShape = targetShape,
                Layers = targetLayers
            };

            var ingredientsToProcess = originalRecipe.Ingredients.AsEnumerable();
            if (excludeSurface) ingredientsToProcess = ingredientsToProcess.Where(i => i is not SurfaceIngredient);

            foreach (var ingredient in ingredientsToProcess)
            {
                var clonedIngredient = ingredient.Clone();
                clonedIngredient.Scale(volumeRatio, areaRatio, creamRatio);
                newRecipe.Ingredients.Add(clonedIngredient);
            }

            return newRecipe;
        }
    }
}