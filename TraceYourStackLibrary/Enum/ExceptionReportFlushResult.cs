namespace TraceYourStackLibrary.Enum
{
    /// <summary>
    /// Indicates the result of the flush operation
    /// </summary>
    public enum ExceptionReportFlushResult
    {
        /// <summary>
        /// All the pending reports were uploaded correctly
        /// </summary>
        Success,

        /// <summary>
        /// The user canceled the token for the flush operation
        /// </summary>
        OperationCanceled,

        /// <summary>
        /// The authorization token wasn't valid
        /// </summary>
        InvalidAuthorizationToken,

        /// <summary>
        /// An error occurred on the remote server
        /// </summary>
        WebServiceError,

        /// <summary>
        /// The last exception was queued correctly but it wasn't possible to flush the pending elements
        /// </summary>
        NetworkConnectionNotAvailable,

        /// <summary>
        /// Something happened during the flush operation
        /// </summary>
        UnknownError
    }
}