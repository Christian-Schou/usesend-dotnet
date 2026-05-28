namespace UseSend;

/// <summary>
///     An attachment to include in an outgoing email.
/// </summary>
public class EmailAttachment
{
    /// <summary>
    ///     File name (e.g. <c>invoice.pdf</c>).
    /// </summary>
    public string Filename { get; set; } = default!;

    /// <summary>
    ///     Base64-encoded file content.
    /// </summary>
    public string Content { get; set; } = default!;
}