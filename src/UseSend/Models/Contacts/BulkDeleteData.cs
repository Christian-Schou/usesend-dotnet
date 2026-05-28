namespace UseSend;

/// <summary>
///     Request body for bulk-deleting contacts.
/// </summary>
public class BulkDeleteData
{
    /// <summary>Contact IDs to delete (1–1000).</summary>
    public List<string> ContactIds { get; set; } = new();
}