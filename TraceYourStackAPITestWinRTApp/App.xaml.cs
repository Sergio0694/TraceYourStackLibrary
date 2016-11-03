using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using TraceYourStackLibrary;

namespace TraceYourStackAPITestWinRTApp
{
    /// <summary>
    ///Fornisce un comportamento specifico dell'applicazione in supplemento alla classe Application predefinita.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Inizializza l'oggetto Application singleton. Si tratta della prima riga del codice creato
        /// eseguita e, come tale, corrisponde all'equivalente logico di main() o WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += (s, e) =>
            {
                Debug.WriteLine("UnhandledException handler");
                PackageVersion pVersion = Package.Current.Id.Version;
                Version version = new Version(pVersion.Major, pVersion.Minor, pVersion.Build, pVersion.Revision);
                TysAPIs.LogException(e.Exception, version);
                Debug.WriteLine("Exception logged");
            };
        }

        /// <summary>
        /// Gets the authorization token for the TraceYourStack library
        /// </summary>
        private const String TraceYourStackAuthorizationToken = "UZz5xTEUjs7YoW2hQhCKxw";

        /// <summary>
        /// Richiamato quando l'applicazione viene avviata normalmente dall'utente.  All'avvio dell'applicazione
        /// verranno utilizzati altri punti di ingresso per aprire un file specifico.
        /// </summary>
        /// <param name="e">Dettagli sulla richiesta e sul processo di avvio.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Library initialization
            ISQLitePlatform platform = new SQLitePlatformWinRT();
            EasClientDeviceInformation info = new EasClientDeviceInformation();
            String device = $"{info.SystemManufacturer} {info.SystemProductName}";
            TysAPIs.InitializeLibrary(platform, new WinRTSettings(), device, TraceYourStackAuthorizationToken);
            Debug.WriteLine("TraceYourStack library initialized");

            // Flush
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            TysAPIs.FlushExceptionsQueueAsync(cts.Token).ContinueWith(t =>
            {
                Debug.WriteLine("Flush method returned");
            }).Forget();

            Frame rootFrame = Window.Current.Content as Frame;

            // Non ripetere l'inizializzazione dell'applicazione se la finestra già dispone di contenuto
            if (rootFrame == null)
            {
                // Creare un frame che agisca da contesto di navigazione e passare alla prima pagina
                rootFrame = new Frame { Language = Windows.Globalization.ApplicationLanguages.Languages[0] };
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: caricare lo stato dall'applicazione sospesa in precedenza
                }

                // Posizionare il frame nella finestra corrente
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Quando lo stack di esplorazione non viene ripristinato, passare alla prima pagina
                // configurando la nuova pagina per passare le informazioni richieste come parametro di
                // navigazione
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Assicurarsi che la finestra corrente sia attiva
            Window.Current.Activate();
        }

        /// <summary>
        /// Chiamato quando la navigazione a una determinata pagina ha esito negativo
        /// </summary>
        /// <param name="sender">Frame la cui navigazione non è riuscita</param>
        /// <param name="e">Dettagli sull'errore di navigazione.</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Richiamato quando l'esecuzione dell'applicazione viene sospesa. Lo stato dell'applicazione viene salvato
        /// senza che sia noto se l'applicazione verrà terminata o ripresa con il contenuto
        /// della memoria ancora integro.
        /// </summary>
        /// <param name="sender">Origine della richiesta di sospensione.</param>
        /// <param name="e">Dettagli relativi alla richiesta di sospensione.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }

    public static class Extensions
    {
        public static void Forget(this Task task) { }
    }
}
