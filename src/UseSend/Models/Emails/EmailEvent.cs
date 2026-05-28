namespace UseSend;

/// <summary>
///     A single email event (delivery tracking entry).
/// </summary>
public class EmailEvent
{
    /// <summary>Email identifier.</summary>
    public string EmailId { get; set; } = default!;

    /// <summary>Event status.</summary>
    public EmailStatus Status { get; set; }

    /// <summary>When the event occurred.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Additional event data (provider-specific).</summary>
    public object? Data { get; set; }
}