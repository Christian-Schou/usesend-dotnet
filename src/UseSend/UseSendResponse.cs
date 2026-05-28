namespace UseSend;

/// <summary>
///     Response from the useSend API.
/// </summary>
public class UseSendResponse
{
    /// <summary />
    public UseSendResponse()
    {
        Success = true;
    }

    /// <summary />
    public UseSendResponse(UseSendException exception)
    {
        Success = false;
        Exception = exception;
    }

    /// <summary>
    ///     Gets whether the invocation was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    ///     Gets the error in case of an unsuccessful invocation.
    /// </summary>
    public UseSendException? Exception { get; }
}

/// <summary>
///     Response from the useSend API that, whenever successful, carries content.
/// </summary>
public class UseSendResponse<T> : UseSendResponse
{
    private readonly T? _value;

    /// <summary />
    public UseSendResponse(T value)
    {
        _value = value;
    }

    /// <summary />
    public UseSendResponse(UseSendException exception)
        : base(exception)
    {
    }

    /// <summary>
    ///     Gets the response content. Only valid when <see cref="UseSendResponse.Success" /> is true.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the invocation failed.
    /// </exception>
    public T Content => _value ??
                        throw new InvalidOperationException(
                            "Response does not contain content. Check Success before accessing Content.");
}