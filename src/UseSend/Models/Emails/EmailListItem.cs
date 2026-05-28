namespace UseSend;

/// <summary>
///     Lightweight email summary returned by <c>GET /v1/emails</c>.
/// </summary>
public class EmailListItem
{
    /// <summary>Email identifier.</summary>
    public string Id { get; set; } = default!;

    /// <summary>Recipient(s).</summary>
    public object To { get; set; } = default!;

    /// <summary>Sender address.</summary>
    public string From { get; set; } = default!;

    /// <summary>Subject line.</summary>
    public string Subject { get; set; } = default!;

    /// <summary>HTML body (nullable).</summary>
    public string? Html { get; set; }

    /// <summary>Plain-text body (nullable).</summary>
    public string? Text { get; set; }

    /// <summary>Reply-to address(es).</summary>
    public object? ReplyTo { get; set; }

    /// <summary>CC address(es).</summary>
    public object? Cc { get; set; }

    /// <summary>BCC address(es).</summary>
    public object? Bcc { get; set; }

    /// <summary>Most recent delivery status.</summary>
    public EmailStatus? LatestStatus { get; set; }

    /// <summary>Scheduled send time (if any).</summary>
    public DateTimeOffset? ScheduledAt { get; set; }

    /// <summary>Domain identifier (if any).</summary>
    public long? DomainId { get; set; }

    /// <summary>When the email was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>When the email was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}