using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CakeCalculatorApp.Models;
using Supabase;

namespace CakeCalculatorApp.Data
{
    public class SupabaseDatabaseService : IDatabaseService
    {
        private readonly Client _supabaseClient;

        public SupabaseDatabaseService(string url, string key)
        {
            var options = new SupabaseOptions { AutoConnectRealtime = true };
            _supabaseClient = new Client(url, key, options);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            try
            {
                await _supabaseClient.InitializeAsync();
                return new List<Recipe>();
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Failed to connect to Supabase: {ex.Message}", ex);
            }
        }

        public async Task SaveRecipeAsync(Recipe recipe)
        {
            try
            {
                await _supabaseClient.InitializeAsync();
                Console.WriteLine($"Saving recipe: {recipe.Name}");
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Failed to save recipe to Supabase: {ex.Message}", ex);
            }
        }
    }
}