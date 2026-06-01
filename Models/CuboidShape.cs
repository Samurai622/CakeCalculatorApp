using System;
using System.Text.Json.Serialization;

namespace CakeCalculatorApp.Models
{
    /// <summary>
    /// Клас, що описує форму класичного прямокутного торта (геометричний паралелепіпед).
    /// Успадковує базовий клас Shape та реалізує його абстрактні методи.
    /// </summary>
    public class CuboidShape : Shape
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        [JsonConstructor]
        public CuboidShape() { }

        public CuboidShape(double length, double width, double height)
        {
            if( length <= 0 || width <= 0 || height <= 0)
                throw new ArgumentException("All dimensions must be greater than zero");

            Length = length;
            Width = width;
            Height = height;
            Name = $"Прямокутна ({length} x {width} см, h={height} см)";
        }

        public override double GetVolume() => Length * Width * Height;
        public override double GetSurfaceArea() => (Length * Width) + 2 * (Length * Height) + 2 * (Width * Height);
        public override double GetBaseArea() => Length * Width;
        public override double GetHeight() => Height;
    }
}