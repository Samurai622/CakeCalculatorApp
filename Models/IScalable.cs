namespace CakeCalculatorApp.Models
{
    /// <summary>
    /// Інтерфейс, що визначає контракт для об'єктів, які можуть бути відмасштабовані 
    /// (перераховані) залежно від нових фізичних параметрів торта.
    /// </summary>
    public interface IScalable
    {
        /// <summary>
        /// Змінює кількісні характеристики об'єкта на основі переданих коефіцієнтів.
        /// </summary>
        /// <param name="volumeRatio">Коефіцієнт зміни загального об'єму.</param>
        /// <param name="areaRatio">Коефіцієнт зміни площі поверхні.</param>
        /// <param name="creamRatio">Коефіцієнт зміни об'єму прошарків крему.</param>
        void Scale(double volumeRatio, double areaRatio, double creamRatio);
    }
}