using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CakeCalculatorApp.ViewModels;

namespace CakeCalculatorApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

        private async void ActionLoad_Click(object? sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (ViewModel.UseCloudDatabase)
            {
                await ViewModel.SyncFromCloudCommand.ExecuteAsync(null);
            }
            else
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel == null) return;

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Відкрити базу рецептів",
                    AllowMultiple = false,
                    FileTypeFilter = new[] { new FilePickerFileType("JSON Files") { Patterns = new[] { "*.json" } } }
                });

                if (files.Count >= 1)
                {
                    await ViewModel.LoadRecipeFromFileAsync(files[0].Path.LocalPath);
                }
            }
        }

        private async void ActionSave_Click(object? sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (ViewModel.UseCloudDatabase)
            {
                await ViewModel.UploadToCloudCommand.ExecuteAsync(null);
            }
            else
            {
                var topLevel = TopLevel.GetTopLevel(this);
                if (topLevel == null) return;

                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Зберегти базу рецептів",
                    DefaultExtension = "json",
                    FileTypeChoices = new[] { new FilePickerFileType("JSON Files") { Patterns = new[] { "*.json" } } }
                });

                if (file != null)
                {
                    await ViewModel.SaveRecipeToFileAsync(file.Path.LocalPath);
                }
            }
        }

        private async void About_Click(object? sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            await aboutWindow.ShowDialog(this);
        }
    }
}