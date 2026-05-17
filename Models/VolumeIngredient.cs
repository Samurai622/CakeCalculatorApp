using System;

namespace CakeCalculatorApp.Models
{
    public class VolumeIngredient : Ingredient
    {
        public override void Scale(double volumeRatio, double areaRatio, double creamRatio)
        {
            Weight = Math.Round(Weight * volumeRatio, 1);
        }

        public override Ingredient Clone()
        {
            return new VolumeIngredient { Name = this.Name, Weight = this.Weight, Unit = this.Unit };
        }
    }
}