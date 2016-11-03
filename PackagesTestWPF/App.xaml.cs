using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.Reflection;
using System.Threading;
using System.Windows;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Generic;
using TraceYourStackLibrary;
using TraceYourStackLibrary.Enum;

namespace TraceYourStackAPITestWPFApp
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the authorization token for the TraceYourStack library
        /// </summary>
        private const String TraceYourStackAuthorizationToken = "dnjsdwdnsko";

        public App()
        {
            // Add the handler to the unhandled exception event
            DispatcherUnhandledException += (s, e) =>
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                TysAPIs.LogException(e.Exception, version);
            };
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Initialize the SQLite platform and the library
            ISQLitePlatform platform = new SQLitePlatformGeneric();
            TysAPIs.InitializeLibrary(platform, TraceYourStackAuthorizationToken);

            // Flush the previous exceptions, if needed
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            ExceptionReportFlushResult result = await TysAPIs.FlushExceptionsQueueAsync(cts.Token);
#if DEBUG
            Debug.WriteLine($"Flush completed > {result}");
#endif
        }
    }
}
