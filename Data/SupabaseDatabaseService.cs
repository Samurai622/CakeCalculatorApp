using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using CakeCalculatorApp.Models;

namespace CakeCalculatorApp.Data
{
    /// <summary>
    /// Сервіс хмарної бази даних. Здійснює асинхронну синхронізацію 
    /// рецептів з віддаленим сервером PostgreSQL (платформа Supabase) через DTO-моделі.
    /// </summary>
    public class SupabaseDatabaseService : IDatabaseService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public SupabaseDatabaseService(string url, string key)
        {
            var options = new Supabase.SupabaseOptions { AutoConnectRealtime = true };
            _supabaseClient = new Supabase.Client(url, key, options);

            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            try
            {
                await _supabaseClient.InitializeAsync();
                
                var response = await _supabaseClient.From<SupabaseRecipeDto>().Get();
                var dtos = response.Models;

                var recipes = new List<Recipe>();
                foreach (var dto in dtos)
                {
                    var recipe = JsonSerializer.Deserialize<Recipe>(dto.RecipeJson, _jsonOptions);
                    if (recipe != null) recipes.Add(recipe);
                }

                return recipes;
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Помилка завантаження з хмари: {ex.Message}", ex);
            }
        }

        public async Task SaveRecipeAsync(Recipe recipe)
        {
            try
            {
                await _supabaseClient.InitializeAsync();
                
                string json = JsonSerializer.Serialize(recipe, _jsonOptions);

                var dto = new SupabaseRecipeDto
                {
                    Name = recipe.Name,
                    RecipeJson = json
                };

                await _supabaseClient.From<SupabaseRecipeDto>().Insert(dto);
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Помилка збереження в хмару: {ex.Message}", ex);
            }
        }
    }
}