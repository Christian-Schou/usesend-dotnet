namespace UseSend;

/// <summary>
///     A useSend campaign.
/// </summary>
public class Campaign
{
    /// <summary>Campaign identifier.</summary>
    public string Id { get; set; } = default!;

    /// <summary>Campaign name.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Sender address.</summary>
    public string From { get; set; } = default!;

    /// <summary>Subject line.</summary>
    public string Subject { get; set; } = default!;

    /// <summary>Preview text shown in email clients.</summary>
    public string? PreviewText { get; set; }

    /// <summary>Contact book this campaign targets.</summary>
    public string? ContactBookId { get; set; }

    /// <summary>HTML email body.</summary>
    public string? Html { get; set; }

    /// <summary>Editor JSON content.</summary>
    public string? Content { get; set; }

    /// <summary>Campaign status.</summary>
    public CampaignStatus Status { get; set; }

    /// <summary>Scheduled send time.</summary>
    public DateTimeOffset? ScheduledAt { get; set; }

    /// <summary>Batch size (emails per batch).</summary>
    public int BatchSize { get; set; }

    /// <summary>Minutes between batches.</summary>
    public int BatchWindowMinutes { get; set; }

    /// <summary>Total recipients.</summary>
    public int Total { get; set; }

    /// <summary>Emails sent.</summary>
    public int Sent { get; set; }

    /// <summary>Emails delivered.</summary>
    public int Delivered { get; set; }

    /// <summary>Emails opened.</summary>
    public int Opened { get; set; }

    /// <summary>Links clicked.</summary>
    public int Clicked { get; set; }

    /// <summary>Unsubscribes.</summary>
    public int Unsubscribed { get; set; }

    /// <summary>Soft bounces.</summary>
    public int Bounced { get; set; }

    /// <summary>Hard bounces.</summary>
    public int HardBounced { get; set; }

    /// <summary>Spam complaints.</summary>
    public int Complained { get; set; }

    /// <summary>Reply-to address(es).</summary>
    public List<string> ReplyTo { get; set; } = new();

    /// <summary>CC address(es).</summary>
    public List<string> Cc { get; set; } = new();

    /// <summary>BCC address(es).</summary>
    public List<string> Bcc { get; set; } = new();

    /// <summary>When the campaign was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>When the campaign was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}