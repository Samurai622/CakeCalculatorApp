using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CakeCalculatorApp.Views
{   
    /// <summary>
    /// Модальне діалогове вікно "Про програму".
    /// Відображає інформацію про версію застосунку, розробника та університет.
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обробник події кліку на кнопку "Закрити".
        /// Закриває поточне модальне вікно та повертає фокус головному вікну програми.
        /// </summary>
        private void Close_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}