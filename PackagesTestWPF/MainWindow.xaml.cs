using System;
using System.Threading.Tasks;
using System.Windows;

namespace TraceYourStackAPITestWPFApp
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static void Crash()
        {
            throw new InvalidOperationException("This is a test Exception for the TraceYourStack library");
        }

        private static async Task<int> NullAsync(int value)
        {
            await Task.Delay(500);
            Crash();
            return value + 1;
        }

        private static Task<int> CalculateAsync(bool flag)
        {
            Func<Task<int>> f = async () =>
            {
                await Task.Delay(1000);
                return await NullAsync(2);
            };
            return f();
        }

        private async void VoidCallback()
        {
            await CalculateAsync(true);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            VoidCallback();
        }
    }
}
