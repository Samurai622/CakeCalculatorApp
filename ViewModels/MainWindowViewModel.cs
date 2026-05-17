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
        
        [ObservableProperty] private bool _excludeSurface = false;

        public ObservableCollection<Recipe> CatalogRecipes { get; } = new();
        public ObservableCollection<Ingredient> OriginalIngredients { get; } = new();
        public ObservableCollection<Ingredient> RecalculatedIngredients { get; } = new();

        public MainWindowViewModel()
        {
            _localDatabase = new LocalJsonDatabaseService();
            _cloudDatabase = new SupabaseDatabaseService("https://your-project.supabase.co", "your-anon-key");
            _calculatorService = new CakeCalculatorService();
            
            OriginalIngredients.Add(new VolumeIngredient { Name = "Борошно", Weight = 500, Unit = "г" });
            OriginalIngredients.Add(new VolumeIngredient { Name = "Цукор", Weight = 200, Unit = "г" });
            OriginalIngredients.Add(new SurfaceIngredient { Name = "Глазур", Weight = 150, Unit = "г" });
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
                    Ingredients = OriginalIngredients.ToList()
                };

                var newRecipe = _calculatorService.CalculateNewRecipe(originalRecipe, targetShape, ExcludeSurface);

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
    }
}