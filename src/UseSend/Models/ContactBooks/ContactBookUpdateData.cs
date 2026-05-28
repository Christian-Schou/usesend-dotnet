namespace UseSend;

/// <summary>
///     Data for updating an existing contact book (all fields optional).
/// </summary>
public class ContactBookUpdateData
{
    /// <summary>New display name.</summary>
    public string? Name { get; set; }

    /// <summary>Emoji icon.</summary>
    public string? Emoji { get; set; }

    /// <summary>Custom properties schema.</summary>
    public Dictionary<string, string>? Properties { get; set; }

    /// <summary>Allowed personalization variable names.</summary>
    public List<string>? Variables { get; set; }

    /// <summary>Enable/disable double opt-in.</summary>
    public bool? DoubleOptInEnabled { get; set; }

    /// <summary>From address for double opt-in emails.</summary>
    public string? DoubleOptInFrom { get; set; }

    /// <summary>Subject for double opt-in email.</summary>
    public string? DoubleOptInSubject { get; set; }

    /// <summary>Email editor JSON content for double opt-in email.</summary>
    public string? DoubleOptInContent { get; set; }
}