using System.Text.Json.Serialization;

namespace CakeCalculatorApp.Models
{
    [JsonDerivedType(typeof(VolumeIngredient), typeDiscriminator: "volume")]
    [JsonDerivedType(typeof(SurfaceIngredient), typeDiscriminator: "surface")]
    [JsonDerivedType(typeof(CreamIngredient), typeDiscriminator: "cream")]
    public abstract class Ingredient : IScalable
    {
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string Unit { get; set; } = "г";

        [JsonIgnore]
        public abstract string IngredientTypeLabel { get; }

        public abstract void Scale(double volumeRatio, double areaRatio, double creamRatio);
        public abstract Ingredient Clone();
    }
}