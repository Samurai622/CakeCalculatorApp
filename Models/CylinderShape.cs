using System;
using System.Text.Json.Serialization;

namespace CakeCalculatorApp.Models
{
    public class CylinderShape : Shape
    {
        public double Radius { get; set; }
        public double Height { get; set; }

        [JsonConstructor]
        public CylinderShape() { }

        public CylinderShape(double radius, double height)
        {
            if( radius <= 0 || height <= 0)
                throw new ArgumentException("Radius and height must be greater than zero");

            Radius = radius;
            Height = height;
            Name = $"Кругла (d={radius * 2} см, h={height} см)";
        }

        public override double GetVolume() => Math.PI * Math.Pow(Radius, 2) * Height;
        public override double GetSurfaceArea() => (Math.PI * Math.Pow(Radius, 2)) + (2 * Math.PI * Radius * Height);
        public override double GetBaseArea() => Math.PI * Math.Pow(Radius, 2);
        public override double GetHeight() => Height;
    }
}