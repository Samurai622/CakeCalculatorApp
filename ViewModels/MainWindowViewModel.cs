using System;
using System.Collections.Generic;
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

        public List<string> AvailableShapes { get; } = new() { "Кругла", "Прямокутна" };
        
        private string _selectedOriginalShape = "Кругла";
        public string SelectedOriginalShape
        {
            get => _selectedOriginalShape;
            set
            {
                SetProperty(ref _selectedOriginalShape, value);
                OnPropertyChanged(nameof(IsOriginalWidthVisible));
            }
        }

        private string _selectedTargetShape = "Прямокутна";
        public string SelectedTargetShape
        {
            get => _selectedTargetShape;
            set
            {
                SetProperty(ref _selectedTargetShape, value);
                OnPropertyChanged(nameof(IsTargetWidthVisible));
            }
        }

        public bool IsOriginalWidthVisible => SelectedOriginalShape == "Прямокутна";
        public bool IsTargetWidthVisible => SelectedTargetShape == "Прямокутна";

        [ObservableProperty] private double _originalParam1 = 20;
        [ObservableProperty] private double _originalParam2 = 15; 
        [ObservableProperty] private double _originalHeight = 10;

        [ObservableProperty] private double _targetParam1 = 25;
        [ObservableProperty] private double _targetParam2 = 20;
        [ObservableProperty] private double _targetHeight = 10;

        [ObservableProperty] private int _originalLayers = 3;
        [ObservableProperty] private int _targetLayers = 3;
        
        [ObservableProperty] private bool _excludeSurface = false;

        public ObservableCollection<Recipe> CatalogRecipes { get; } = new();
        public ObservableCollection<Ingredient> OriginalIngredients { get; } = new();
        public ObservableCollection<Ingredient> RecalculatedIngredients { get; } = new();

        [ObservableProperty] private string _newIngredientName = "";
        [ObservableProperty] private double _newIngredientWeight = 100;
        
        public List<string> AvailableUnits { get; } = new() { "г", "кг", "мл", "л", "шт", "ч.л.", "ст.л.", "стакан" };
        [ObservableProperty] private string _selectedUnit = "г";
        
        public List<string> IngredientTypes { get; } = new() { "Тісто (Об'єм)", "Крем між коржами", "Глазур (Поверхня)" };
        [ObservableProperty] private string _selectedIngredientType = "Тісто (Об'єм)";
        public MainWindowViewModel()
        {
            _localDatabase = new LocalJsonDatabaseService();
            _cloudDatabase = new SupabaseDatabaseService("https://your-project.supabase.co", "your-anon-key");
            _calculatorService = new CakeCalculatorService();
        }

        private Shape CreateShape(string shapeType, double p1, double p2, double h)
        {
            return shapeType switch
            {
                "Кругла" => new CylinderShape(p1, h),
                "Прямокутна" => new CuboidShape(p1, p2, h),
                _ => new CylinderShape(p1, h)
            };
        }

        [RelayCommand]
        private void Calculate()
        {
            try
            {
                Shape originalShape = CreateShape(SelectedOriginalShape, OriginalParam1, OriginalParam2, OriginalHeight);
                Shape targetShape = CreateShape(SelectedTargetShape, TargetParam1, TargetParam2, TargetHeight);

                var originalRecipe = new Recipe
                {
                    Name = "Торт_0",
                    CakeShape = originalShape,
                    Layers = OriginalLayers,
                    Ingredients = OriginalIngredients.ToList()
                };

                var newRecipe = _calculatorService.CalculateNewRecipe(originalRecipe, targetShape, TargetLayers, ExcludeSurface);

                RecalculatedIngredients.Clear();
                foreach (var item in newRecipe.Ingredients) RecalculatedIngredients.Add(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        [RelayCommand]
        private void SortByWeight()
        {
            var sorted = RecalculatedIngredients.OrderByDescending(i => i.Weight).ToList();
            RecalculatedIngredients.Clear();
            foreach (var item in sorted) RecalculatedIngredients.Add(item);
        }

        [RelayCommand]
        private void FilterVolumeOnly()
        {
            var filtered = RecalculatedIngredients.Where(i => i is VolumeIngredient).ToList();
            RecalculatedIngredients.Clear();
            foreach (var item in filtered) RecalculatedIngredients.Add(item);
        }

        [RelayCommand]
        private void AddIngredient()
        {
            if (string.IsNullOrWhiteSpace(NewIngredientName) || NewIngredientWeight <= 0)
                return;

            Ingredient newIng;
            if (SelectedIngredientType == "Глазур (Поверхня)")
                newIng = new SurfaceIngredient { Name = NewIngredientName, Weight = NewIngredientWeight, Unit = SelectedUnit };
            else if (SelectedIngredientType == "Крем між коржами")
                newIng = new CreamIngredient { Name = NewIngredientName, Weight = NewIngredientWeight, Unit = SelectedUnit };
            else
                newIng = new VolumeIngredient { Name = NewIngredientName, Weight = NewIngredientWeight, Unit = SelectedUnit };

            OriginalIngredients.Add(newIng);
            
            NewIngredientName = "";
        }

        [RelayCommand]
        private void ClearIngredients()
        {
            OriginalIngredients.Clear();
            RecalculatedIngredients.Clear();
        }
    }
}