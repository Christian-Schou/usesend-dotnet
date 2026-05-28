namespace UseSend;

/// <summary>
///     Email message to send via useSend.
/// </summary>
public class EmailMessage
{
    /// <summary>
    ///     Recipient address(es). String or array of strings.
    /// </summary>
    public object To { get; set; } = default!;

    /// <summary>
    ///     Sender address (must use a verified useSend domain).
    /// </summary>
    public string From { get; set; } = default!;

    /// <summary>
    ///     Email subject. Optional when <see cref="TemplateId" /> is provided.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    ///     HTML body.
    /// </summary>
    public string? Html { get; set; }

    /// <summary>
    ///     Plain-text body.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     Reply-to address(es).
    /// </summary>
    public object? ReplyTo { get; set; }

    /// <summary>
    ///     CC address(es).
    /// </summary>
    public object? Cc { get; set; }

    /// <summary>
    ///     BCC address(es).
    /// </summary>
    public object? Bcc { get; set; }

    /// <summary>
    ///     ID of a template from the useSend dashboard.
    ///     When provided, <see cref="Subject" /> becomes optional.
    /// </summary>
    public string? TemplateId { get; set; }

    /// <summary>
    ///     Template variables (key/value pairs) merged into the template.
    /// </summary>
    public Dictionary<string, string>? Variables { get; set; }

    /// <summary>
    ///     Custom email headers.
    /// </summary>
    public Dictionary<string, string>? Headers { get; set; }

    /// <summary>
    ///     File attachments (max 10).
    /// </summary>
    public List<EmailAttachment>? Attachments { get; set; }

    /// <summary>
    ///     Schedule the email for a future time (ISO 8601).
    /// </summary>
    public DateTimeOffset? ScheduledAt { get; set; }

    /// <summary>
    ///     The email ID this message is a reply to, forming a thread.
    /// </summary>
    public string? InReplyToId { get; set; }
}