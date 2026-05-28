namespace UseSend.Webhooks;

/// <summary>
///     Thrown when webhook signature verification fails.
/// </summary>
public class WebhookException : Exception
{
    /// <inheritdoc />
    public WebhookException(string message) : base(message)
    {
    }
}