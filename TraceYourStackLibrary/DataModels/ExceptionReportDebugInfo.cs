using System;
using JetBrains.Annotations;
using TraceYourStackLibrary.SQLite.Models;

namespace TraceYourStackLibrary.DataModels
{
    /// <summary>
    /// Gets a model that contains all the debug info on an exception report
    /// </summary>
    public sealed class ExceptionReportDebugInfo
    {
        /// <summary>
        /// Gets the exception type for this report
        /// </summary>
        [NotNull]
        public String ExceptionType { get; }

        /// <summary>
        /// Gets the optional message of this exception
        /// </summary>
        [CanBeNull]
        public String Message { get; }

        /// <summary>
        /// Gets the HResult of this exception
        /// </summary>
        public int HResult { get; }

        /// <summary>
        /// Gets the optional help link for this exception
        /// </summary>
        [CanBeNull]
        public String HelpLink { get; }

        /// <summary>
        /// Gets the stack trace for this exception
        /// </summary>
        [CanBeNull]
        public String StackTrace { get; }

        /// <summary>
        /// Gets the app version retrieved when the crash occurred
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public Version AppVersion { get; }

        /// <summary>
        /// Gets the time of this exception
        /// </summary>]
        public DateTime CrashTime { get; }

        /// <summary>
        /// Gets or sets the number of times this kind of exception was thrown
        /// </summary>
        public int ExceptionTypeOccurrencies { get; }

        /// <summary>
        /// Gets or sets the most recent time this exception was thrown
        /// </summary>
        public DateTime RecentCrashTime { get; }

        /// <summary>
        /// Gets or sets the less recent time this exception was thrown
        /// </summary>
        public DateTime LessRecentCrashTime { get; }

        /// <summary>
        /// Gets or sets the minimum version number that caused this exception
        /// </summary>
        [NotNull]
        public Version MinExceptionVersion { get; }

        /// <summary>
        /// Gets or sets the higher version number that caused this exception
        /// </summary>
        [NotNull]
        public Version MaxExceptionVersion { get; }

        #region Internal constructor

        // Creates a new immutable instance to share with other projects
        private ExceptionReportDebugInfo([NotNull] ExceptionReport report)
        {
            ExceptionType = report.ExceptionType;
            Message = report.Message;
            HResult = report.HResult;
            HelpLink = report.HelpLink;
            StackTrace = report.StackTrace;
            AppVersion = Version.Parse(report.AppVersion);
            CrashTime = DateTime.FromBinary(report.CrashTime);
            ExceptionTypeOccurrencies = report.ExceptionTypeOccurrencies;
            RecentCrashTime = DateTime.FromBinary(report.RecentCrashTime);
            LessRecentCrashTime = DateTime.FromBinary(report.LessRecentCrashTime);
            MinExceptionVersion = Version.Parse(report.MinExceptionVersion);
            MaxExceptionVersion = Version.Parse(report.MaxExceptionVersion);
        }

        /// <summary>
        /// Creates a new immutable instance of an exception report
        /// </summary>
        /// <param name="report">The source exception report</param>
        internal static ExceptionReportDebugInfo New([NotNull] ExceptionReport report)
        {
            return new ExceptionReportDebugInfo(report);
        }

        #endregion
    }
}
