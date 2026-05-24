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

        [ObservableProperty] private string _greeting = "Режим: Локальний файл (JSON)";
        [ObservableProperty] private bool _useCloudDatabase;

        public string SyncModeButtonText => UseCloudDatabase ? "📂 Local Sync" : "☁️ Supabase Sync";
        public string LoadButtonText => UseCloudDatabase ? "☁️ Завантажити з хмари" : "📂 Відкрити файл";
        public string SaveButtonText => UseCloudDatabase ? "🚀 Відправити в хмару" : "💾 Зберегти файл";

        public IDatabaseService CurrentDatabase => UseCloudDatabase ? _cloudDatabase : _localDatabase;

        [ObservableProperty] private string _originalRecipeName = "";
        
        public ObservableCollection<Recipe> AvailableRecipes { get; } = new();
        
        private Recipe? _selectedRecipe;
        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                SetProperty(ref _selectedRecipe, value);
                OnRecipeSelected();
            }
        }

        [ObservableProperty] private bool _isCustomRecipe = true;


        public List<string> AvailableShapes { get; } = new() { "Кругла", "Прямокутна" };
        
        private string _selectedOriginalShape = "Кругла";
        public string SelectedOriginalShape
        {
            get => _selectedOriginalShape;
            set
            {
                SetProperty(ref _selectedOriginalShape, value);
                OnPropertyChanged(nameof(IsOriginalWidthVisible));
                OnPropertyChanged(nameof(OriginalParam1Label));
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
                OnPropertyChanged(nameof(TargetParam1Label));
            }
        }

        public bool IsOriginalWidthVisible => SelectedOriginalShape == "Прямокутна";
        public bool IsTargetWidthVisible => SelectedTargetShape == "Прямокутна";
        public string OriginalParam1Label => SelectedOriginalShape == "Кругла" ? "Радіус (см):" : "Довжина (см):";
        public string TargetParam1Label => SelectedTargetShape == "Кругла" ? "Радіус (см):" : "Довжина (см):";

        [ObservableProperty] private double _originalParam1 = 20;
        [ObservableProperty] private double _originalParam2 = 15; 
        [ObservableProperty] private double _originalHeight = 10;

        [ObservableProperty] private double _targetParam1 = 25;
        [ObservableProperty] private double _targetParam2 = 20;
        [ObservableProperty] private double _targetHeight = 10;

        [ObservableProperty] private int _originalLayers = 3;
        [ObservableProperty] private int _targetLayers = 3;
        
        [ObservableProperty] private bool _excludeSurface = false;

        public ObservableCollection<Ingredient> OriginalIngredients { get; } = new();
        public ObservableCollection<Ingredient> RecalculatedIngredients { get; } = new();

        [ObservableProperty] private string _newIngredientName = "";
        [ObservableProperty] private double _newIngredientWeight = 100;
        
        public List<string> AvailableUnits { get; } = new() { "г", "кг", "мл", "л", "шт", "ч.л.", "ст.л.", "стакан" };
        [ObservableProperty] private string _selectedUnit = "г";

        public List<string> IngredientTypes { get; } = new() { "Тісто (Об'єм)", "Крем між коржами", "Глазур (Поверхня)" };
        [ObservableProperty] private string _selectedIngredientType = "Тісто (Об'єм)";

        [ObservableProperty] private double _originalTotal;
        [ObservableProperty] private double _recalculatedTotal;
        
        private Ingredient? _selectedOriginalIngredient;
        public Ingredient? SelectedOriginalIngredient
        {
            get => _selectedOriginalIngredient;
            set
            {
                SetProperty(ref _selectedOriginalIngredient, value);
                OnPropertyChanged(nameof(HasSelectedIngredient));
            }
        }

        public bool HasSelectedIngredient => SelectedOriginalIngredient != null;
        public bool HasIngredients => OriginalIngredients.Count > 0;

        public MainWindowViewModel()
        {
            _localDatabase = new LocalJsonDatabaseService();
            
            string supabaseUrl = "https://yvwqbaoqlicpnvmfcmrj.supabase.co";
            string supabaseKey = "sb_publishable_Uga9iIl9o7O5c7bdZcz60g_F4kjPNMJ"; 
            
            _cloudDatabase = new SupabaseDatabaseService(supabaseUrl, supabaseKey);
            _calculatorService = new CakeCalculatorService();

            InitializeRecipeListAsync();
        }

        private async System.Threading.Tasks.Task InitializeRecipeListAsync()
        {
            await LoadRecipesIntoDropdownAsync();
        }

        private async System.Threading.Tasks.Task LoadRecipesIntoDropdownAsync()
        {
            try
            {
                AvailableRecipes.Clear();
                
                var customChoice = new Recipe { Name = "🎂 Створити свій рецепт..." };
                AvailableRecipes.Add(customChoice);

                var recipesFromDb = await CurrentDatabase.GetRecipesAsync();
                
                foreach (var recipe in recipesFromDb)
                {
                    AvailableRecipes.Add(recipe);
                }

                SelectedRecipe = AvailableRecipes[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження списку: {ex.Message}");
            }
        }

        private void OnRecipeSelected()
        {
            if (SelectedRecipe == null) return;

            if (SelectedRecipe.Name == "🎂 Створити свій рецепт...")
            {
                IsCustomRecipe = true;
                OriginalRecipeName = "";
                ClearIngredients();
            }
            else
            {
                IsCustomRecipe = false;
                OriginalRecipeName = SelectedRecipe.Name;

                OriginalIngredients.Clear();
                foreach (var ing in SelectedRecipe.Ingredients)
                {
                    OriginalIngredients.Add(ing.Clone());
                }

                if (SelectedRecipe.CakeShape is CylinderShape cyl)
                {
                    SelectedOriginalShape = "Кругла";
                    OriginalParam1 = cyl.Radius;
                    OriginalHeight = cyl.Height;
                }
                else if (SelectedRecipe.CakeShape is CuboidShape cub)
                {
                    SelectedOriginalShape = "Прямокутна";
                    OriginalParam1 = cub.Length;
                    OriginalParam2 = cub.Width;
                    OriginalHeight = cub.Height;
                }
                OriginalLayers = SelectedRecipe.Layers;
                UpdateTotals();
            }
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

        private void UpdateTotals()
        {
            OriginalTotal = Math.Round(OriginalIngredients.Sum(i => i.Weight), 1);
            RecalculatedTotal = Math.Round(RecalculatedIngredients.Sum(i => i.Weight), 1);
            OnPropertyChanged(nameof(HasIngredients));
        }


        [RelayCommand]
        private async System.Threading.Tasks.Task ToggleDatabaseModeAsync()
        {
            UseCloudDatabase = !UseCloudDatabase;
            
            OnPropertyChanged(nameof(SyncModeButtonText));
            OnPropertyChanged(nameof(LoadButtonText));
            OnPropertyChanged(nameof(SaveButtonText));
            
            Greeting = UseCloudDatabase ? "Режим: Хмарна БД (Supabase)" : "Режим: Локальний файл (JSON)";

            await LoadRecipesIntoDropdownAsync();
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
                    Name = string.IsNullOrWhiteSpace(OriginalRecipeName) ? "Без назви" : OriginalRecipeName,
                    CakeShape = originalShape,
                    Layers = OriginalLayers,
                    Ingredients = OriginalIngredients.ToList()
                };

                var newRecipe = _calculatorService.CalculateNewRecipe(originalRecipe, targetShape, TargetLayers, ExcludeSurface);

                RecalculatedIngredients.Clear();
                foreach (var item in newRecipe.Ingredients) RecalculatedIngredients.Add(item);
                
                UpdateTotals();
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
            UpdateTotals();
        }

        [RelayCommand]
        private void FilterVolumeOnly()
        {
            var filtered = RecalculatedIngredients.Where(i => i is VolumeIngredient).ToList();
            RecalculatedIngredients.Clear();
            foreach (var item in filtered) RecalculatedIngredients.Add(item);
            UpdateTotals();
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
            UpdateTotals(); 
        }

        [RelayCommand]
        private void DeleteSelectedIngredient()
        {
            if (SelectedOriginalIngredient != null)
            {
                OriginalIngredients.Remove(SelectedOriginalIngredient);
                UpdateTotals();
            }
        }

        [RelayCommand]
        private void ClearIngredients()
        {
            OriginalIngredients.Clear();
            RecalculatedIngredients.Clear();
            UpdateTotals();
        }

        [RelayCommand]
        private void LoadForEdit()
        {
            if (SelectedOriginalIngredient != null)
            {
                NewIngredientName = SelectedOriginalIngredient.Name;
                NewIngredientWeight = SelectedOriginalIngredient.Weight;
                SelectedUnit = SelectedOriginalIngredient.Unit;

                if (SelectedOriginalIngredient is SurfaceIngredient)
                    SelectedIngredientType = "Глазур (Поверхня)";
                else if (SelectedOriginalIngredient is CreamIngredient)
                    SelectedIngredientType = "Крем між коржами";
                else
                    SelectedIngredientType = "Тісто (Об'єм)";
            }
        }

        [RelayCommand]
        private void UpdateIngredient()
        {
            if (SelectedOriginalIngredient == null || string.IsNullOrWhiteSpace(NewIngredientName) || NewIngredientWeight <= 0)
                return;

            int index = OriginalIngredients.IndexOf(SelectedOriginalIngredient);
            if (index == -1) return;

            Ingredient updatedIng;
            if (SelectedIngredientType == "Глазур (Поверхня)")
                updatedIng = new SurfaceIngredient { Name = NewIngredientName, Weight = NewIngredientWeight, Unit = SelectedUnit };
            else if (SelectedIngredientType == "Крем між коржами")
                updatedIng = new CreamIngredient { Name = NewIngredientName, Weight = NewIngredientWeight, Unit = SelectedUnit };
            else
                updatedIng = new VolumeIngredient { Name = NewIngredientName, Weight = NewIngredientWeight, Unit = SelectedUnit };

            OriginalIngredients[index] = updatedIng;
            NewIngredientName = "";
            UpdateTotals();
        }

        
        public async System.Threading.Tasks.Task SaveRecipeToFileAsync(string filePath)
        {
            try
            {
                if (OriginalIngredients.Count == 0) return;

                var fileService = new LocalJsonDatabaseService(filePath);

                var recipeToSave = new Recipe
                {
                    Name = string.IsNullOrWhiteSpace(OriginalRecipeName) ? "Збережений рецепт" : OriginalRecipeName,
                    CakeShape = CreateShape(SelectedOriginalShape, OriginalParam1, OriginalParam2, OriginalHeight),
                    Layers = OriginalLayers,
                    Ingredients = OriginalIngredients.ToList()
                };

                await fileService.SaveRecipeAsync(recipeToSave);
                Greeting = $"Рецепт успішно збережено у {System.IO.Path.GetFileName(filePath)}";
                
                await LoadRecipesIntoDropdownAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження: {ex.Message}");
                Greeting = "Помилка збереження файлу!";
            }
        }

        public async System.Threading.Tasks.Task LoadRecipeFromFileAsync(string filePath)
        {
            try
            {
                var fileService = new LocalJsonDatabaseService(filePath);
                var recipes = (await fileService.GetRecipesAsync()).ToList();

                if (recipes.Count > 0)
                {
                    SelectedRecipe = recipes.Last();
                    Greeting = $"Рецепт завантажено з {System.IO.Path.GetFileName(filePath)}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження: {ex.Message}");
                Greeting = "Помилка завантаження файлу!";
            }
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task SyncFromCloudAsync()
        {
            try
            {
                Greeting = "Завантаження з хмари...";
                await LoadRecipesIntoDropdownAsync(); 
                Greeting = "Синхронізовано з Supabase!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Greeting = "Помилка хмари!";
            }
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task UploadToCloudAsync()
        {
            try
            {
                if (OriginalIngredients.Count == 0) return;
                Greeting = "Відправка у хмару...";

                var recipeToSave = new Recipe
                {
                    Name = string.IsNullOrWhiteSpace(OriginalRecipeName) ? "Рецепт зі спільноти" : OriginalRecipeName,
                    CakeShape = CreateShape(SelectedOriginalShape, OriginalParam1, OriginalParam2, OriginalHeight),
                    Layers = OriginalLayers,
                    Ingredients = OriginalIngredients.ToList()
                };

                await _cloudDatabase.SaveRecipeAsync(recipeToSave);
                Greeting = "Успішно відправлено у Supabase!";
                
                await LoadRecipesIntoDropdownAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Greeting = "Помилка відправки!";
            }
        }
    }
}