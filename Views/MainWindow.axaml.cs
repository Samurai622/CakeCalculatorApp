using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace CakeCalculatorApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OpenFile_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Відкрити базу рецептів",
            AllowMultiple = false
        };
        dialog.Filters.Add(new FileDialogFilter { Name = "JSON Files", Extensions = { "json" } });

        var result = await dialog.ShowAsync(this);
        if(result != null && result.Length > 0)
        {
            Console.WriteLine($"Вибрано файл: {result[0]}");
        }
    }

    private async void SaveFile_click(object? sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Зберегти базу рецептів",
            DefaultExtension = "json",
        };
        dialog.Filters.Add(new FileDialogFilter { Name = "JSON Files", Extensions = { "json" } });

        var result = await dialog.ShowAsync(this);
        if(result != null)
        {
            Console.WriteLine($"Збережено файл: {result}");
        }
    }
}