namespace UseSend.Identity;

/// <summary>
///     Options for <see cref="UseSendEmailSender" /> and <see cref="UseSendEmailSender{TUser}" />.
/// </summary>
public sealed class EmailSenderOptions
{
    /// <summary>The address emails are sent from.</summary>
    public string FromAddress { get; }

    /// <summary>Optional display name shown alongside <see cref="FromAddress" />.</summary>
    public string? FromName { get; }

    /// <param name="fromAddress">The address emails are sent from.</param>
    /// <param name="fromName">Optional display name, e.g. "My App".</param>
    public EmailSenderOptions(string fromAddress, string? fromName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fromAddress);
        FromAddress = fromAddress;
        FromName = fromName;
    }

    internal string FormattedFrom =>
        FromName is { Length: > 0 } name ? $"{name} <{FromAddress}>" : FromAddress;
}
