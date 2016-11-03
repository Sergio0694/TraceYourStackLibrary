using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace TraceYourStackLibrary.SQLite.Models
{
    /// <summary>
    /// A simple JSON wrapper for an exception report
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ExceptionReportWrapper
    {
        [JetBrains.Annotations.NotNull]
        [JsonProperty("Content", Required = Required.Always)]
        public ExceptionReport Report { get; }

        // Private constructor
        private ExceptionReportWrapper([JetBrains.Annotations.NotNull] ExceptionReport report)
        {
            Report = report;
        }

        /// <summary>
        /// Implicit converter into a wrapped report
        /// </summary>
        /// <param name="report">The report to wrap</param>
        public static implicit operator ExceptionReportWrapper([JetBrains.Annotations.NotNull] ExceptionReport report)
        {
            return new ExceptionReportWrapper(report);
        }
    }

    /// <summary>
    /// The table that contains the queued exception reports to upload
    /// </summary>
    [Table("ExceptionReports")]
    [JsonObject(MemberSerialization.OptIn)]
    internal class ExceptionReport
    {
        #region SQLite columns

        /// <summary>
        /// Gets the uid of the entry
        /// </summary>
        [Column(nameof(Uid)), PrimaryKey]
        public String Uid { get; set; }

        /// <summary>
        /// Gets the exception type for this report
        /// </summary>
        [Column(nameof(ExceptionType)), NotNull]
        [JsonProperty("Type", Required = Required.Always)]
        public String ExceptionType { get; set; }

        /// <summary>
        /// Gets the optional message of this exception
        /// </summary>
        [Column(nameof(Message))]
        [JsonProperty(nameof(Message), DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Message { get; set; }

        /// <summary>
        /// Gets the HResult of this exception
        /// </summary>
        [Column(nameof(HResult))]
        [JsonProperty(nameof(HResult), DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HResult { get; set; }

        /// <summary>
        /// Gets the optional help link for this exception
        /// </summary>
        [Column(nameof(HelpLink))]
        [JsonProperty(nameof(HelpLink), DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String HelpLink { get; set; }

        /// <summary>
        /// Gets the stack trace for this exception
        /// </summary>
        [Column(nameof(StackTrace))]
        [JsonProperty(nameof(StackTrace), DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String StackTrace { get; set; }

        /// <summary>
        /// Gets the app version retrieved when the crash occurred
        /// </summary>
        [Column(nameof(AppVersion)), NotNull]
        [JsonProperty(nameof(AppVersion), Required = Required.Always)]
        public String AppVersion { get; set; }

        /// <summary>
        /// Gets the time of this exception
        /// </summary>
        [Column(nameof(CrashTime)), NotNull]
        public long CrashTime { get; set; }

        /// <summary>
        /// Gets a value that indicates whether or not this report has already been flushed
        /// </summary>
        [Column(nameof(Flushed)), NotNull, Default]
        public byte Flushed { get; set; }

        #endregion

        #region Parameters

        /// <summary>
        /// Gets the name of the current device
        /// </summary>
        [Ignore]
        [JsonProperty(nameof(Device), Required = Required.Always)]
        public String Device => TysAPIs.DeviceName;

        /// <summary>
        /// Gets the DateTime that represents the crash time
        /// </summary>
        [Ignore]
        [JsonProperty(nameof(CrashDateTime), Required = Required.Always)]
        public DateTime CrashDateTime => DateTime.FromBinary(CrashTime);

        /// <summary>
        /// Gets or sets the number of times this kind of exception was thrown
        /// </summary>
        [Ignore]
        public int ExceptionTypeOccurrencies { get; set; }

        /// <summary>
        /// Gets or sets the most recent time this exception was thrown
        /// </summary>
        [Ignore]
        public long RecentCrashTime { get; set; }

        /// <summary>
        /// Gets or sets the less recent time this exception was thrown
        /// </summary>
        [Ignore]
        public long LessRecentCrashTime { get; set; }

        /// <summary>
        /// Gets or sets the minimum version number that caused this exception
        /// </summary>
        [Ignore]
        public String MinExceptionVersion { get; set; }

        /// <summary>
        /// Gets or sets the higher version number that caused this exception
        /// </summary>
        [Ignore]
        public String MaxExceptionVersion { get; set; }

        #endregion

        /// <summary>
        /// Creates a new instance with a new uid and the right parameters for the JSON serialization
        /// </summary>
        /// <param name="type">The exception type</param>
        /// <param name="message">The exception message</param>
        /// <param name="hResult">The exception HResult, if available</param>
        /// <param name="helplink">The optional help link</param>
        /// <param name="stackTrace">The exception stack trace, if available</param>
        /// <param name="version">The app version in use</param>
        /// <param name="crashTime">The crash time for this exception</param>
        public static ExceptionReport NewInstance(
            [JetBrains.Annotations.NotNull] String type,
            [JetBrains.Annotations.CanBeNull] String message, int hResult,
            [JetBrains.Annotations.CanBeNull] String helplink,
            [JetBrains.Annotations.CanBeNull] String stackTrace,
            [JetBrains.Annotations.NotNull] String version, long crashTime)
        {
            return new ExceptionReport
            {
                Uid = Guid.NewGuid().ToString(),
                ExceptionType = type,
                Message = String.IsNullOrEmpty(message) ? null : message,
                HResult = hResult,
                HelpLink = String.IsNullOrEmpty(helplink) ? null : helplink,
                StackTrace = String.IsNullOrEmpty(stackTrace) ? null : stackTrace,
                AppVersion = version,
                CrashTime = crashTime
            };
        }
    }
}
