using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TraceYourStackAPITestWinRTApp
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
