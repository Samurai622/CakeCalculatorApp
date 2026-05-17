using System;

namespace CakeCalculatorApp.Models
{
    public class CuboidShape : Shape
    {
        public double Length { get; }
        public double Width { get; }
        public double Height { get; }

        public CuboidShape(double length, double width, double height)
        {
            if( length <= 0 || width <= 0 || height <= 0)
                throw new ArgumentException("All dimensions must be greater than zero");

            Length = length;
            Width = width;
            Height = height;
            Name = $"Rectangular ({length} x {width} cm, h={height} cm)";
        }

        public override double GetVolume()
        {
            return Length * Width * Height;
        }

        public override double GetSurfaceArea()
        {
            double topArea = Length * Width;
            double sideArea1 = 2 * (Length * Height);
            double sideArea2 = 2 * (Width * Height);
            return topArea + sideArea1 + sideArea2;
        }

        public override double GetBaseArea()
        {
            return Length * Width;
        }

        public override double GetHeight()
        {
            return Height;
        }
    }
}