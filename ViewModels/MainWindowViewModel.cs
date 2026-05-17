using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        private string _greeting = "Welcome to Cake Calculator!";

        [ObservableProperty]
        private bool _useCloudDatabase;

        public IDatabaseService CurrentDatabase => UseCloudDatabase ? _cloudDatabase : _localDatabase;

        public ObservableCollection<Recipe> CatalogRecipes { get; } = new();
        public ObservableCollection<Ingredient> OriginalIngredients { get; } = new();
        public ObservableCollection<Ingredient> RecalculatedIngredients { get; } = new();

        public MainWindowViewModel()
        {
            _localDatabase = new LocalJsonDatabaseService();
            
            _cloudDatabase = new SupabaseDatabaseService("https://your-project.supabase.co", "your-anon-key");
            
            _calculatorService = new CakeCalculatorService();
            
            OriginalIngredients.Add(new VolumeIngredient { Name = "Flour", Weight = 500, Unit = "g" });
            OriginalIngredients.Add(new VolumeIngredient { Name = "Sugar", Weight = 200, Unit = "g" });
            OriginalIngredients.Add(new SurfaceIngredient { Name = "Chocolate Glaze", Weight = 150, Unit = "g" });
        }

        partial void OnUseCloudDatabaseChanged(bool value)
        {
            Greeting = value ? "Using Supabase Cloud Database" : "Using Local JSON File";
        }
    }
}