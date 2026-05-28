namespace UseSend;

/// <summary>
///     A useSend contact book (list of subscribers).
/// </summary>
public class ContactBook
{
    /// <summary>Contact book identifier.</summary>
    public string Id { get; set; } = default!;

    /// <summary>Display name.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Team identifier.</summary>
    public long TeamId { get; set; }

    /// <summary>Custom properties schema for this book.</summary>
    public Dictionary<string, string> Properties { get; set; } = new();

    /// <summary>Allowed personalization variable names for contacts in this book.</summary>
    public List<string> Variables { get; set; } = new();

    /// <summary>Emoji icon associated with this book.</summary>
    public string? Emoji { get; set; }

    /// <summary>Whether double opt-in is enabled for new contacts.</summary>
    public bool? DoubleOptInEnabled { get; set; }

    /// <summary>From address used for double opt-in emails.</summary>
    public string? DoubleOptInFrom { get; set; }

    /// <summary>Subject line for the double opt-in confirmation email.</summary>
    public string? DoubleOptInSubject { get; set; }

    /// <summary>JSON content for the double opt-in email.</summary>
    public string? DoubleOptInContent { get; set; }

    /// <summary>Number of contacts in this book.</summary>
    public ContactBookCount? Count { get; set; }

    /// <summary>When the contact book was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>When the contact book was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
///     Contact count summary for a contact book.
/// </summary>
public class ContactBookCount
{
    /// <summary>Total number of contacts.</summary>
    public int Contacts { get; set; }
}