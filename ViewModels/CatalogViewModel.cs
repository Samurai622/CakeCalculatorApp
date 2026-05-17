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

        [RelayCommand]
        private void SortByName()
        {
            var sortedList = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OrderBy(Recipes, r => r.Name));
            Recipes.Clear();
            foreach (var recipe in sortedList)
            {
                Recipes.Add(recipe);
            }
        }

        [RelayCommand]
        private void FilterRoundCakesOnly()
        {
            var filteredList = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(Recipes, r => r.CakeShape is CylinderShape));
            Recipes.Clear();
            foreach (var recipe in filteredList)
            {
                Recipes.Add(recipe);
            }
        }
    }  
}