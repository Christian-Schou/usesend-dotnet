namespace UseSend;

/// <summary>
///     Data for creating a new contact.
/// </summary>
public class ContactData
{
    /// <summary>Email address. Required.</summary>
    public string Email { get; set; } = default!;

    /// <summary>First name.</summary>
    public string? FirstName { get; set; }

    /// <summary>Last name.</summary>
    public string? LastName { get; set; }

    /// <summary>Custom properties (key/value pairs).</summary>
    public Dictionary<string, string>? Properties { get; set; }

    /// <summary>Whether the contact should be subscribed. Defaults to true.</summary>
    public bool? Subscribed { get; set; }
}