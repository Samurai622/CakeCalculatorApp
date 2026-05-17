namespace CakeCalculatorApp.Models
{
    public abstract class Ingredient : IScalable
    {
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string Unit { get; set; } = "g";

        public abstract void Scale( double volumeRatio, double areaRatio, double creamRatio);

        public abstract Ingredient Clone();
    }
}