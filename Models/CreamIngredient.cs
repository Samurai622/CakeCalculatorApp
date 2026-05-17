using System;

namespace CakeCalculatorApp.Models
{
    public class CreamIngredient : Ingredient
    {
        public override void Scale(double volumeRatio, double areaRatio, double creamRatio)
        {
            Weight = Math.Round(Weight * creamRatio, 1);
        }

        public override Ingredient Clone()
        {
            return new CreamIngredient { Name = this.Name, Weight = this.Weight, Unit = this.Unit };
        }
    }
}