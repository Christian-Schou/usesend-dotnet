using Microsoft.Extensions.Options;

namespace UseSend;

/// <summary>
///     Configuration options for <see cref="UseSendClient" />.
/// </summary>
public class UseSendClientOptions
{
    /// <summary>
    ///     Base URL for the useSend API.
    /// </summary>
    /// <remarks>
    ///     Defaults to <c>https://app.usesend.com/api/</c>.
    ///     Override this when targeting a self-hosted useSend instance,
    ///     e.g. <c>https://send.mycompany.com/api/</c>.
    /// </remarks>
    public string ApiUrl { get; set; } = "https://app.usesend.com/api/";

    /// <summary>
    ///     Bearer token used to authenticate against the useSend API.
    /// </summary>
    /// <remarks>
    ///     If not set here, the client falls back to the <c>USESEND_API_KEY</c>
    ///     environment variable.
    /// </remarks>
    public string ApiToken { get; set; } = default!;

    /// <summary>
    ///     When <c>true</c> (the default), API failures throw a
    ///     <see cref="UseSendException" />. When <c>false</c>, failures are
    ///     returned as an unsuccessful <see cref="UseSendResponse" />.
    /// </summary>
    public bool ThrowExceptions { get; set; } = true;
}

/// <summary />
internal class OptionsSnapshot<T> : IOptionsSnapshot<T>
    where T : class
{
    internal OptionsSnapshot(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public T Get(string? name)
    {
        return Value;
    }
}