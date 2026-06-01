using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Data
{
    /// <summary>
    /// Сервіс локальної бази даних. Серіалізує об'єкти у JSON-формат 
    /// та зберігає їх у файловій системі комп'ютера.
    /// </summary>
    public class LocalJsonDatabaseService : IDatabaseService
    {
        private readonly string _filePath;

        private readonly JsonSerializerOptions _jsonOptions;

        public LocalJsonDatabaseService(string filePath = "recipes_db.json")
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            try
            {
                if(!File.Exists(_filePath))
                {
                    return new List<Recipe>();
                }

                using FileStream stream = File.OpenRead(_filePath);
                var recipes = await JsonSerializer.DeserializeAsync<List<Recipe>>(stream, _jsonOptions);
                return recipes ?? new List<Recipe>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to load recipes from local JSON file. {ex.Message}", ex);
            }
        }
        public async Task SaveRecipeAsync(Recipe recipe)
        {
            try
            {
                var existingRecipes = new List<Recipe>(await GetRecipesAsync());
                
                var index = existingRecipes.FindIndex(r => r.Id == recipe.Id);
                if (index != -1)
                    existingRecipes[index] = recipe;
                else
                    existingRecipes.Add(recipe);

                using FileStream stream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(stream, existingRecipes, _jsonOptions);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to save recipe to file: {ex.Message}", ex);
            }
        }
    }
}