namespace UseSend;

/// <summary>
///     A contact within a useSend contact book.
/// </summary>
public class Contact
{
    /// <summary>Contact identifier.</summary>
    public string Id { get; set; } = default!;

    /// <summary>Email address.</summary>
    public string Email { get; set; } = default!;

    /// <summary>First name.</summary>
    public string? FirstName { get; set; }

    /// <summary>Last name.</summary>
    public string? LastName { get; set; }

    /// <summary>Whether the contact is subscribed.</summary>
    public bool Subscribed { get; set; } = true;

    /// <summary>Custom contact properties (key/value pairs).</summary>
    public Dictionary<string, string> Properties { get; set; } = new();

    /// <summary>The contact book this contact belongs to.</summary>
    public string ContactBookId { get; set; } = default!;

    /// <summary>When the contact was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>When the contact was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}