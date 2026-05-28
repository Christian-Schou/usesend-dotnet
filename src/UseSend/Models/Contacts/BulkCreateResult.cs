namespace UseSend;

/// <summary>
///     Result of a bulk contact create operation.
/// </summary>
public class BulkCreateResult
{
    /// <summary>Human-readable message.</summary>
    public string Message { get; set; } = default!;

    /// <summary>Number of contacts created.</summary>
    public int Count { get; set; }
}