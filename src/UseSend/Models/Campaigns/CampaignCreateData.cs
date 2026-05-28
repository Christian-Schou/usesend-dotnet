namespace UseSend;

/// <summary>
///     Data for creating a new campaign.
/// </summary>
public class CampaignCreateData
{
    /// <summary>Campaign name. Required.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Sender address. Required.</summary>
    public string From { get; set; } = default!;

    /// <summary>Subject line. Required.</summary>
    public string Subject { get; set; } = default!;

    /// <summary>Contact book to send to. Required.</summary>
    public string ContactBookId { get; set; } = default!;

    /// <summary>HTML email body.</summary>
    public string? Html { get; set; }

    /// <summary>Editor JSON content.</summary>
    public string? Content { get; set; }

    /// <summary>Preview text shown in email clients.</summary>
    public string? PreviewText { get; set; }

    /// <summary>Reply-to address(es).</summary>
    public object? ReplyTo { get; set; }

    /// <summary>CC address(es).</summary>
    public object? Cc { get; set; }

    /// <summary>BCC address(es).</summary>
    public object? Bcc { get; set; }

    /// <summary>Send immediately after creation.</summary>
    public bool? SendNow { get; set; }

    /// <summary>Schedule for a specific time (ISO 8601 or natural language).</summary>
    public string? ScheduledAt { get; set; }

    /// <summary>Number of emails per batch (1–100000).</summary>
    public int? BatchSize { get; set; }
}