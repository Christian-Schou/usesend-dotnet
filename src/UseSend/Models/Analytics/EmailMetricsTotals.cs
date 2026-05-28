namespace UseSend;

/// <summary>
///     Aggregated email metric totals.
/// </summary>
public class EmailMetricsTotals
{
    /// <summary>Emails sent.</summary>
    public int Sent { get; set; }

    /// <summary>Emails delivered.</summary>
    public int Delivered { get; set; }

    /// <summary>Emails opened.</summary>
    public int Opened { get; set; }

    /// <summary>Links clicked.</summary>
    public int Clicked { get; set; }

    /// <summary>Bounces.</summary>
    public int Bounced { get; set; }

    /// <summary>Spam complaints.</summary>
    public int Complained { get; set; }
}