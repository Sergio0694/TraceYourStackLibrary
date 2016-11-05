using System;
using JetBrains.Annotations;
using TraceYourStackLibrary.Helpers.LocalStorage;
using TraceYourStackLibrary.SQLite.Models;

namespace TraceYourStackLibrary.Helpers
{
    /// <summary>
    /// A static class that manages the local settings on the current device
    /// </summary>
    internal static class Settings
    {
        /// <summary>
        /// Gets the local settings instance to use
        /// </summary>
        private static ISettingsManager AppSettings => TysAPIs.SettingsManager;

        #region Setting Constants

        /* ==========================
         * Exception info constants
         * ======================= */
        private const String ExceptionType = nameof(ExceptionType);
        private const String ExceptionMessage = nameof(ExceptionMessage);
        private const String StackTrace = nameof(StackTrace);
        private const String ExceptionHResult = nameof(ExceptionHResult);
        private const String ExceptionHelpLink = nameof(ExceptionHelpLink);
        private const String ExceptionAppVersion = nameof(ExceptionAppVersion);
        private const String ExceptionTime = nameof(ExceptionTime);

        #endregion

        /// <summary>
        /// Saves the info on the current exception in the local settings
        /// </summary>
        /// <param name="e">The exception that was thrown by the app</param>
        /// <param name="version">The current app version</param>
        public static void LogException([NotNull] Exception e, [NotNull] Version version)
        {
            AppSettings.Clear();
            AppSettings.AddOrUpdateValue(ExceptionType, e.GetType().ToString());
            AppSettings.AddOrUpdateValue(ExceptionMessage, e.Message);
            AppSettings.AddOrUpdateValue(StackTrace, e.StackTrace);
            AppSettings.AddOrUpdateValue(ExceptionHResult, e.HResult);
            AppSettings.AddOrUpdateValue(ExceptionHelpLink, e.HelpLink);
            AppSettings.AddOrUpdateValue(ExceptionAppVersion, version.ToString());
            AppSettings.AddOrUpdateValue(ExceptionTime, DateTime.Now.ToBinary());
        }

        /// <summary>
        /// Tries to get the last stored exception, if present
        /// </summary>
        public static ExceptionReport TryGetLastSavedException()
        {
            // Check if there's a pending exception
            if (!AppSettings.ContainsKey(ExceptionType)) return null;

            // Make sure the required info are present
            String
                type = AppSettings.GetValueOrDefault<String>(ExceptionType),
                version = AppSettings.GetValueOrDefault<String>(ExceptionAppVersion);
            if (type == null || version == null)
            {
                AppSettings.Clear();
                return null;
            }

            // Parse and return the new instance
            return ExceptionReport.NewInstance(type,
                AppSettings.GetValueOrDefault<String>(ExceptionMessage),
                AppSettings.GetValueOrDefault<int>(ExceptionHResult),
                AppSettings.GetValueOrDefault<String>(ExceptionHelpLink),
                AppSettings.GetValueOrDefault<String>(StackTrace),
                version, AppSettings.GetValueOrDefault<long>(ExceptionTime));
        }
    }
}