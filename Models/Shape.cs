namespace CakeCalculatorApp.Models
{
    public abstract class Shape
    {
        public string Name { get; set;} = string.Empty;

        public abstract double GetVolume();

        public abstract double GetSurfaceArea();

        public abstract double GetBaseArea();

        public abstract double GetHeight();
    }
}