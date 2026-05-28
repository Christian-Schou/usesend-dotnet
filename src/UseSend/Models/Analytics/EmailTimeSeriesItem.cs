namespace UseSend;

/// <summary>
///     A single data point in the email time series.
/// </summary>
public class EmailTimeSeriesItem
{
    /// <summary>Date label (e.g. "2024-01-15").</summary>
    public string Date { get; set; } = default!;

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