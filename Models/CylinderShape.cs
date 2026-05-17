using System;

namespace CakeCalculatorApp.Models
{
    public class CylinderShape : Shape
    {
        public double Radius { get; }
        public double Height { get; }

        public CylinderShape(double radius, double height)
        {
            if( radius <= 0 || height <= 0)
                throw new ArgumentException("Radius and height must be greater than zero");

            Radius = radius;
            Height = height;
            Name = $"Round (d={radius * 2} cm, h={height} cm)";
        }

        public override double GetVolume()
        {
            return Math.PI * Math.Pow(Radius, 2) * Height;
        }

        public override double GetSurfaceArea()
        {
            double topArea = Math.PI * Math.Pow(Radius, 2);
            double sideArea = 2 * Math.PI * Radius * Height;
            return topArea + sideArea;
        }

        public override double GetBaseArea()
        {
            return Math.PI * Math.Pow(Radius, 2);
        }

        public override double GetHeight()
        {
            return Height;
        }
    }
}