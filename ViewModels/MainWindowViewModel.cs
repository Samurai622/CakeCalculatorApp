using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CakeCalculatorApp.Models;
using CakeCalculatorApp.Data;
using CakeCalculatorApp.Services;

namespace CakeCalculatorApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IDatabaseService _localDatabase;
        private readonly IDatabaseService _cloudDatabase;
        private readonly ICakeCalculatorService _calculatorService;

        [ObservableProperty] private string _greeting = "Калькулятор Тортів (Офлайн)";
        [ObservableProperty] private bool _useCloudDatabase;

        public IDatabaseService CurrentDatabase => UseCloudDatabase ? _cloudDatabase : _localDatabase;

        [ObservableProperty] private double _originalRadius = 10;
        [ObservableProperty] private double _originalHeight = 10;
        [ObservableProperty] private double _targetLength = 20;
        [ObservableProperty] private double _targetWidth = 15;
        [ObservableProperty] private double _targetHeight = 10;
        [ObservableProperty] private bool _excludeSurface = false;

        public ObservableCollection<Recipe> CatalogRecipes { get; } = new();
        public ObservableCollection<Ingredient> OriginalIngredients { get; } = new();
        public ObservableCollection<Ingredient> RecalculatedIngredients { get; } = new();

        public MainWindowViewModel()
        {
            _localDatabase = new LocalJsonDatabaseService();
            _cloudDatabase = new SupabaseDatabaseService("https://your-project.supabase.co", "your-anon-key");
            _calculatorService = new CakeCalculatorService();
            
            // Тестові інгредієнти
            OriginalIngredients.Add(new VolumeIngredient { Name = "Борошно", Weight = 500, Unit = "г" });
            OriginalIngredients.Add(new VolumeIngredient { Name = "Цукор", Weight = 200, Unit = "г" });
            OriginalIngredients.Add(new SurfaceIngredient { Name = "Шоколадна глазур", Weight = 150, Unit = "г" });
        }

        partial void OnUseCloudDatabaseChanged(bool value)
        {
            Greeting = value ? "Режим: Хмарна БД (Supabase)" : "Режим: Локальний файл (JSON)";
        }

        [RelayCommand]
        private void Calculate()
        {
            try
            {
                var originalRecipe = new Recipe
                {
                    Name = "Торт_0",
                    CakeShape = new CylinderShape(OriginalRadius, OriginalHeight),
                    Ingredients = OriginalIngredients.ToList()
                };

                var targetShape = new CuboidShape(TargetLength, TargetWidth, TargetHeight);
                var newRecipe = _calculatorService.CalculateNewRecipe(originalRecipe, targetShape, ExcludeSurface);

                RecalculatedIngredients.Clear();
                foreach (var item in newRecipe.Ingredients)
                {
                    RecalculatedIngredients.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}