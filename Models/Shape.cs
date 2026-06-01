using System.Text.Json.Serialization;

namespace CakeCalculatorApp.Models
{
    /// <summary>
    /// Базовий абстрактний клас, що представляє геометричну форму торта.
    /// Визначає загальний інтерфейс для обчислення об'єму та площ.
    /// </summary>
    [JsonDerivedType(typeof(CylinderShape), typeDiscriminator: "cylinder")]
    [JsonDerivedType(typeof(CuboidShape), typeDiscriminator: "cuboid")]
    public abstract class Shape
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Обчислює загальний об'єм геометричної фігури.
        /// </summary>
        /// <returns>Значення об'єму в кубічних сантиметрах.</returns>
        public abstract double GetVolume();

        /// <summary>
        /// Обчислює площу зовнішньої поверхні (без урахування дна) для покриття глазур'ю.
        /// </summary>
        /// <returns>Площа поверхні у квадратних сантиметрах.</returns>
        public abstract double GetSurfaceArea();

        /// <summary>
        /// Обчислює площу основи фігури (одного коржа) для розрахунку кількості крему.
        /// </summary>
        /// <returns>Площа основи у квадратних сантиметрах.</returns>
        public abstract double GetBaseArea();

        /// <summary>
        /// Повертає загальну висоту геометричної фігури.
        /// </summary>
        /// <returns>Висота в сантиметрах.</returns>
        public abstract double GetHeight();
    }
}