namespace UseSend;

/// <summary>
///     Query parameters for listing contacts in a contact book.
/// </summary>
public class ContactListQuery
{
    /// <summary>Filter by email address(es), comma-separated.</summary>
    public string? Emails { get; set; }

    /// <summary>Filter by contact IDs, comma-separated.</summary>
    public string? Ids { get; set; }

    /// <summary>Page number.</summary>
    public int? Page { get; set; }

    /// <summary>Results per page.</summary>
    public int? Limit { get; set; }
}