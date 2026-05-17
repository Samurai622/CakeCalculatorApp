using System;

namespace CakeCalculatorApp.Models
{
    public class SurfaceIngredient : Ingredient
    {
        public override void Scale(double volumeRatio, double areaRatio)
        {
            Weight = Math.Round(Weight * areaRatio, 1);
        }

        public override Ingredient Clone()
        {
            return new  SurfaceIngredient { Name = this.Name, Weight = this.Weight, Unit = this.Unit };
        }
    }
}