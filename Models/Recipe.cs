using System;
using System.Collections.Generic;

namespace CakeCalculatorApp.Models
{
    /// <summary>
    /// Клас-модель, що представляє цілісний кулінарний рецепт торта.
    /// Демонструє принцип композиції (включає геометричну форму та список інгредієнтів).
    /// </summary>
    public class Recipe
    {
         /// <summary>
        /// Унікальний ідентифікатор рецепта (згенерується автоматично).
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Назва рецепта, яка відображається у випадаючому списку користувача.
        /// </summary>
        public string Name { get; set; } = string.Empty;    

        /// <summary>
        /// Геометрична форма торта (базовий абстрактний тип Shape).
        /// </summary>
        public Shape? CakeShape { get; set; }

        /// <summary>
        /// Кількість коржів у торті (використовується для розрахунку крему).
        /// </summary>
        public int Layers {get; set; } = 1;

        /// <summary>
        /// Поліморфна колекція інгредієнтів (тісто, глазур, крем), з яких складається торт.
        /// </summary>
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}