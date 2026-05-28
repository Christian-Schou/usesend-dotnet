namespace UseSend;

/// <summary>
///     Result of a domain deletion request.
/// </summary>
public class DomainDeleteResult
{
    /// <summary>Domain identifier.</summary>
    public long Id { get; set; }

    /// <summary>Whether the deletion was successful.</summary>
    public bool Success { get; set; }

    /// <summary>Human-readable message.</summary>
    public string Message { get; set; } = default!;
}