using System;

namespace CakeCalculatorApp.Models
{
    public class SurfaceIngredient : Ingredient
    {
        /// <summary>
        /// Перевизначений метод масштабування. 
        /// Використовує відповідний коефіцієнт для перерахунку власної ваги.
        /// </summary>
        public override void Scale(double volumeRatio, double areaRatio, double creamRatio)
        {
            Weight = Math.Round(Weight * areaRatio, 1);
        }

        public override Ingredient Clone()
        {
            return new  SurfaceIngredient { Name = this.Name, Weight = this.Weight, Unit = this.Unit };
        }

        public override string IngredientTypeLabel => "Глазур (Поверхня)";
    }
}