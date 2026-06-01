using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace CakeCalculatorApp.Models
{
    /// <summary>
    /// Об'єкт передачі даних (Data Transfer Object) для роботи з хмарною базою Supabase.
    /// Використовується для конвертації складної поліморфної моделі Recipe у формат бази даних (PostgreSQL).
    /// </summary>
    [Table("cloud_recipes")]
    public class SupabaseRecipeDto : BaseModel
    {
        /// <summary>
        /// Первинний ключ таблиці бази даних (тип UUID).
        /// </summary>
        [PrimaryKey("id", false)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Назва рецепта, збережена у текстовій колонці для швидкого пошуку.
        /// </summary>
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Зсеріалізований об'єкт рецепта (включаючи всі форми та інгредієнти) у форматі JSON.
        /// Зберігається у PostgreSQL-колонці типу JSONB.
        /// </summary>
        [Column("recipe_json")]
        public string RecipeJson { get; set; } = string.Empty;

        /// <summary>
        /// Дата та час створення або останнього оновлення рецепта у хмарі.
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}