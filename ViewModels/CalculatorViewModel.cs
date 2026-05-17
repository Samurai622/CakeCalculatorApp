using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CakeCalculatorApp.Models;
using CakeCalculatorApp.Services;

namespace CakeCalculatorApp.ViewModels
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        private readonly ICakeCalculatorService _calculatorService;

        [ObservableProperty] private double _targetLength = 20;
        [ObservableProperty] private double _targetWidth = 15;
        [ObservableProperty] private double _targetHeight = 10;
        [ObservableProperty] private bool _excludeSurface = false;

        public ObservableCollection<Ingredient> RecalculatedIngredients { get; } = new();

        public CalculatorViewModel(ICakeCalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [RelayCommand]
        private void Calculate(Recipe originalRecipe)
        {
            try
            {
                if(originalRecipe == null) return;

                var targetShape = new CuboidShape(TargetLength, TargetWidth, TargetHeight);

                var newRecipe = _calculatorService.CalculateNewRecipe(originalRecipe, targetShape, ExcludeSurface);

                RecalculatedIngredients.Clear();
                foreach (var ingredient in newRecipe.Ingredients)
                {
                    RecalculatedIngredients.Add(ingredient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}