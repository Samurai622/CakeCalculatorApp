using System;
using System.Linq;
using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Services
{
    public class CakeCalculatorService : ICakeCalculatorService
    {
        public Recipe CalculateNewRecipe(Recipe originalRecipe, Shape targetShape, bool excludeSurface)
        {
            if(originalRecipe == null || originalRecipe.CakeShape == null)
                throw new ArgumentNullException("Original recipe and its cake shape cannot be null.");
            
            if(targetShape == null)
                throw new ArgumentNullException("Target shape cannot be null.");

            double originalVolume = originalRecipe.CakeShape.GetVolume();
            double originalArea = originalRecipe.CakeShape.GetSurfaceArea();

            if(originalVolume <= 0 || originalArea <= 0)
                throw new ArgumentException("Original recipe must have a positive volume and surface area.");

            double volumeRatio = targetShape.GetVolume() / originalVolume;
            double areaRatio = targetShape.GetSurfaceArea() / originalArea;

            var newRecipe = new Recipe
            {
                Name = $"{originalRecipe.Name} (Recalculated for {targetShape.Name})",
                CakeShape = targetShape,
            };

            var ingredientsToProcess = originalRecipe.Ingredients.AsEnumerable();

            if(excludeSurface)
            {
                ingredientsToProcess = ingredientsToProcess.Where(i => i is not IsSurfaceIngredient);
            }

            foreach (var ingredient in ingredientsToProcess)
            {
                var clonedIngredient = ingredient.Clone();

                clonedIngredient.Scale(volumeRatio, areaRatio);

                newRecipe.Ingredients.Add(clonedIngredient);
            }

            return newRecipe;
        }
    }
}