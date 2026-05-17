using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CakeCalculatorApp.Models;
using CakeCalculatorApp.Data;

namespace CakeCalculatorApp.ViewModels
{
    public partial class CatalogViewModel : ViewModelBase
    {
        private IDatabaseService _databaseService;
        public ObservableCollection<Recipe> Recipes { get; } = new ();
        public CatalogViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void UpdateDatabaseService(IDatabaseService newService)
        {
            _databaseService = newService;
        }

        [RelayCommand]
        private async LoadRecipes()
        {
            Recipes.Clear();
        }
    }
}