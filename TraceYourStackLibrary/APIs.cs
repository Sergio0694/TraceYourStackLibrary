using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SQLite.Net.Interop;

namespace TraceYourStackLibrary
{
    /// <summary>
    /// A wrapper class with all the public APIs exposed by the library
    /// </summary>
    public static class APIs
    {
        /// <summary>
        /// Gets the current SQLite platform to use
        /// </summary>
        internal static ISQLitePlatform Platform { get; private set; }

        /// <summary>
        /// Initializes the SQLite platform to use in the PCL
        /// </summary>
        /// <param name="platform">The current device platform</param>
        public static void InitializeSQLitePlatform([NotNull] ISQLitePlatform platform)
        {
            if (Platform != null) throw new InvalidOperationException("The SQLite platform has already been initialized");
            Platform = platform;
        }

        public static void LogException(Exception e)
        {
            
        }

        public static async Task FlushExceptionsQueueAsync([NotNull] String authorizationToken)
        {
            
        }
    }
}
