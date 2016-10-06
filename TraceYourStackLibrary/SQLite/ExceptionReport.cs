using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace TraceYourStackLibrary.SQLite
{
    /// <summary>
    /// The table that contains the queued exception reports to upload
    /// </summary>
    [Table("ExceptionReports")]
    [JsonObject(MemberSerialization.OptIn)]
    internal class ExceptionReport
    {
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
        /// Gets the stack trace for this exception
        /// </summary>
        [Column(nameof(StackTrace))]
        [JsonProperty(nameof(StackTrace), DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String StackTrace { get; set; }

        /// <summary>
        /// Gets the app version retrieved when the crash occurred
        /// </summary>
        [Column(nameof(AppVersion))]
        [JsonProperty(nameof(AppVersion), Required = Required.Always)]
        public String AppVersion { get; set; }

        /// <summary>
        /// Gets the time of this exception
        /// </summary>
        [Column(nameof(CrashTime)), NotNull]
        [JsonProperty(nameof(CrashTime), Required = Required.Always)]
        public long CrashTime { get; set; }
    }
}
