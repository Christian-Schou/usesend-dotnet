namespace UseSend;

/// <summary>
///     Data for updating an existing contact (all fields optional).
/// </summary>
public class ContactUpdateData
{
    /// <summary>First name.</summary>
    public string? FirstName { get; set; }

    /// <summary>Last name.</summary>
    public string? LastName { get; set; }

    /// <summary>Custom properties (key/value pairs).</summary>
    public Dictionary<string, string>? Properties { get; set; }

    /// <summary>Subscription status.</summary>
    public bool? Subscribed { get; set; }
}