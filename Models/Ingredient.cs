using System.Text.Json.Serialization;

namespace CakeCalculatorApp.Models
{
    /// <summary>
    /// Абстрактний базовий клас для всіх інгредієнтів рецепта.
    /// Забезпечує поліморфну поведінку при перерахунку пропорцій.
    /// </summary>
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
        
        /// <summary>
        /// Створює точну (глибоку) копію поточного інгредієнта для використання у новому рецепті.
        /// </summary>
        /// <returns>Новий об'єкт-спадкоємець класу Ingredient.</returns>
        public abstract Ingredient Clone();
    }
}