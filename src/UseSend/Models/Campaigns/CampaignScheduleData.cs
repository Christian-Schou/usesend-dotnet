namespace UseSend;

/// <summary>
///     Data for scheduling a campaign.
/// </summary>
public class CampaignScheduleData
{
    /// <summary>
    ///     Schedule for a specific time (ISO 8601 or natural language, e.g. "tomorrow 9am").
    /// </summary>
    public string? ScheduledAt { get; set; }

    /// <summary>Send immediately.</summary>
    public bool? SendNow { get; set; }

    /// <summary>Number of emails per batch (1–100000).</summary>
    public int? BatchSize { get; set; }
}