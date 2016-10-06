using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PCLStorage;
using SQLite.Net;
using SQLite.Net.Async;
using TraceYourStackLibrary.Helpers;

namespace TraceYourStackLibrary.SQLite
{
    internal static class SQLiteManager
    {
        #region Constants

        private const String DatabaseFolderName = "TraceYourStackData";

        private const String ExceptionsDatabaseName = "ExceptionsQueueDatabase.db";

        #endregion

        #region Database initialization

        /// <summary>
        /// Gets the database connection in use
        /// </summary>
        private static SQLiteAsyncConnection _Connection;

        /// <summary>
        /// Checks if the given Table is present in the database, and it creates it if it doesn't exist
        /// </summary>
        /// <typeparam name="T">The class that represents the database table</typeparam>
        private static async Task<bool> EnsureTablePresent<T>(SQLiteAsyncConnection connection) where T : class, new()
        {
            try
            {
                await connection.Table<T>().FirstOrDefaultAsync();
                return true;
            }
            catch (SQLiteException)
            {
                //The table doesn't exist
                return false;
            }
        }

        /// <summary>
        /// Loads a database up and connects to it, using a backup database if the target one isn't available
        /// </summary>
        public static async Task EnsureDatabaseInitializedAsync()
        {
            // Initial check
            if (_Connection != null) return;

            // Try to get the database in use
            IFile database = await FileSystemHelper.TryGetFileAsync(ExceptionsDatabaseName, DatabaseFolderName);

            // Connect to the database
            if (APIs.Platform == null) throw new InvalidOperationException("The SQLite platform hasn't been initialized");
            SQLiteConnectionString connectionString = new SQLiteConnectionString(database.Path, true);
            SQLiteConnectionWithLock lockConnection = new SQLiteConnectionWithLock(APIs.Platform, connectionString);
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(() => lockConnection);

            // Make sure the table exists
            if (!await EnsureTablePresent<ExceptionReport>(connection)) await connection.CreateTableAsync<ExceptionReport>();

            // Store the reference to the database in use
            _Connection = connection;
        }

        #endregion
    }
}
