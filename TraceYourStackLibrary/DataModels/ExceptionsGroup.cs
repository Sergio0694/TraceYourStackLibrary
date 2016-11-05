using System;
using JetBrains.Annotations;

namespace TraceYourStackLibrary.DataModels
{
    /// <summary>
    /// Represents the header of a group of saved exceptions
    /// </summary>
    public sealed class ExceptionsGroup
    {
        /// <summary>
        /// Gets the application version for this group of exceptions
        /// </summary>
        [NotNull]
        public Version AppVersion { get; }

        /// <summary>
        /// Gets the number of saved exceptions for this app version
        /// </summary>
        public int ExceptionsNumber { get; }

        internal ExceptionsGroup([NotNull] Version version, int entries)
        {
            AppVersion = version;
            ExceptionsNumber = entries;
        }
    }
}
