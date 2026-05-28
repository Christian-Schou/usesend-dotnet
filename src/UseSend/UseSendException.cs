namespace UseSend;

/// <summary>
///     Exception thrown (or carried in <see cref="UseSendResponse" />) when a useSend API call fails.
/// </summary>
public class UseSendException : Exception
{
    /// <summary />
    public UseSendException(int statusCode, string? apiError, string message)
        : base(message)
    {
        StatusCode = statusCode;
        ApiError = apiError;
    }

    /// <summary />
    public UseSendException(int statusCode, string? apiError, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ApiError = apiError;
    }

    /// <summary>
    ///     HTTP status code returned by the API.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    ///     Error message returned by the API.
    /// </summary>
    public string? ApiError { get; }
}