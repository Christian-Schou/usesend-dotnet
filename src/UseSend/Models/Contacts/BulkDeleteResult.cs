namespace UseSend;

/// <summary>
///     Result of a bulk contact delete operation.
/// </summary>
public class BulkDeleteResult
{
    /// <summary>Whether the deletion was successful.</summary>
    public bool Success { get; set; }

    /// <summary>Number of contacts deleted.</summary>
    public int Count { get; set; }
}