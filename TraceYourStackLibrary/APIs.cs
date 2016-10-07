using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SQLite.Net.Interop;
using TraceYourStackLibrary.Enum;
using TraceYourStackLibrary.Helpers;
using TraceYourStackLibrary.SQLite;

namespace TraceYourStackLibrary
{
    /// <summary>
    /// A wrapper class with all the public APIs exposed by the library
    /// </summary>
    public static class APIs
    {
        #region Parameters

        /// <summary>
        /// Gets the current SQLite platform to use
        /// </summary>
        internal static ISQLitePlatform Platform { get; private set; }

        /// <summary>
        /// Gets the authorization token for the web requests to the service
        /// </summary>
        internal static String AuthorizationToken { get; private set; }

        #endregion

        /// <summary>
        /// <para>Initializes the SQLite platform to use in the PCL and stores the authorization token for the app,</para>
        /// <para>call this method during the app startup</para>
        /// </summary>
        /// <param name="platform">The current device platform</param>
        /// <param name="authorizationToken">The authorization token for the current app</param>
        public static void InitializeLibrary([NotNull] ISQLitePlatform platform, [NotNull] String authorizationToken)
        {
            if (Platform != null || AuthorizationToken != null) throw new InvalidOperationException("The library has already been initialized");
            Platform = platform;
            AuthorizationToken = authorizationToken;
        }

        #region Logging APIs

        /// <summary>
        /// Logs the last thrown exception, call this method from the UnhandledException event handler
        /// </summary>
        /// <param name="e">The new exception</param>
        /// <param name="version">The current app version</param>
        public static void LogException([NotNull] Exception e, [NotNull] Version version) => Settings.LogException(e, version);

        /// <summary>
        /// Flushes all the pending exception reports stored on the device
        /// </summary>
        /// <param name="token">The cancellation token for the operation</param>
        public static async Task<ExceptionReportFlushResult> FlushExceptionsQueueAsync(CancellationToken token)
        {
            try
            {
                // Get the last exception report and add it into the database
                ExceptionReport exception = Settings.TryGetLastSavedException();
                if (exception != null)
                {
                    // Store the new exception
                    await SQLiteManager.StoreNewReportAsync(exception);

                    // Try to flush the new exception
                    ExceptionReportFlushResult flushResult = await WebAPIsHelper.TryFlushReportAsync(AuthorizationToken, exception, token);
                    if (flushResult != ExceptionReportFlushResult.Success) return flushResult;

                    // In case of success, mark the new report as flushed
                    await SQLiteManager.MarkReportAsFlushedAsync(exception);

                    // Check the token
                    if (token.IsCancellationRequested) return ExceptionReportFlushResult.OperationCanceled;
                }
                
                // Try to flush the pending reports
                IEnumerable<ExceptionReport> pending = await SQLiteManager.GetPendingReportsAsync();
                foreach (ExceptionReport report in pending)
                {
                    // Check the token
                    if (token.IsCancellationRequested) return ExceptionReportFlushResult.OperationCanceled;

                    // Try to flush the report
                    ExceptionReportFlushResult flushResult = await WebAPIsHelper.TryFlushReportAsync(AuthorizationToken, report, token);
                    if (flushResult != ExceptionReportFlushResult.Success) return flushResult;

                    // Mark the report as flushed
                    await SQLiteManager.MarkReportAsFlushedAsync(report);
                }

                // Confirm that everything worked fine
                return ExceptionReportFlushResult.Success;
            }
            catch
            {
                // Something bad happened
                return ExceptionReportFlushResult.UnknownError;
            }
        }

        #endregion
    }
}
