namespace UseSend;

/// <summary>
///     Data for creating a new contact book.
/// </summary>
public class ContactBookData
{
    /// <summary>Display name. Required.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Emoji icon.</summary>
    public string? Emoji { get; set; }

    /// <summary>Custom properties schema.</summary>
    public Dictionary<string, string>? Properties { get; set; }

    /// <summary>Allowed personalization variable names for contacts.</summary>
    public List<string>? Variables { get; set; }

    /// <summary>Enable double opt-in for new contacts.</summary>
    public bool? DoubleOptInEnabled { get; set; }

    /// <summary>From address for double opt-in emails (must use verified domain).</summary>
    public string? DoubleOptInFrom { get; set; }

    /// <summary>Subject for double opt-in confirmation email.</summary>
    public string? DoubleOptInSubject { get; set; }

    /// <summary>Email editor JSON content for double opt-in email.</summary>
    public string? DoubleOptInContent { get; set; }
}