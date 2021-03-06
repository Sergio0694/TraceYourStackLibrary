﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TraceYourStackLibrary.Enum;
using TraceYourStackLibrary.SQLite.Models;

namespace TraceYourStackLibrary.Helpers
{
    /// <summary>
    /// A static class that manages the POST operations to the web service
    /// </summary>
    internal static class WebAPIsHelper
    {
        #region Constants

        /// <summary>
        /// Gets the API URL to use
        /// </summary>
        private const String RemoteAPIPostURL = "http://127.0.0.1:3000/upload_stack_trace";

        /// <summary>
        /// Gets the success code
        /// </summary>
        private const String SuccessCode = "200";

        /// <summary>
        /// Gets a generic error code from the web service
        /// </summary>
        private const String ErrorCode = "400";

        /// <summary>
        /// Gets the error code if the app token is invalid
        /// </summary>
        private const String InvalidTokenCode = "403";

        #endregion

        /// <summary>
        /// Tries to flush an exception report to the web service
        /// </summary>
        /// <param name="authorizationToken">The token for the current application</param>
        /// <param name="report">The exception report to flush</param>
        /// <param name="token">The cancellation token for the operation</param>
        public static async Task<ExceptionReportFlushResult> TryFlushReportAsync([NotNull] String authorizationToken, [NotNull] ExceptionReport report, CancellationToken token)
        {
            // Serialize the report into JSON
            ExceptionReportWrapper wrapper = report;
            String json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            
            // Prepare the Http client and its content
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorizationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Flush the report to the web service
            try
            {
                // POST the report
                HttpResponseMessage response = await client.PostAsync(new Uri(RemoteAPIPostURL), content, token).ContinueWith(t => t.GetAwaiter().GetResult(), token);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    return token.IsCancellationRequested ? ExceptionReportFlushResult.OperationCanceled : ExceptionReportFlushResult.NetworkConnectionNotAvailable;
                }

                // Make sure the operation was successfull
                String code = await response.Content.ReadAsStringAsync();
                if (code?.Equals(SuccessCode) == true) return ExceptionReportFlushResult.Success;
                return code?.Equals(InvalidTokenCode) == true
                    ? ExceptionReportFlushResult.InvalidAuthorizationToken
                    : ExceptionReportFlushResult.WebServiceError;

            }
            catch (OperationCanceledException)
            {
                // Token canceled
                return ExceptionReportFlushResult.OperationCanceled;
            }
            catch (Exception)
            {
                // Most likely a network connection error
                return ExceptionReportFlushResult.NetworkConnectionNotAvailable;
            }
        }
    }
}
