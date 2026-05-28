using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Campaign lifecycle status.
/// </summary>
[JsonConverter(typeof(CampaignStatusConverter))]
public enum CampaignStatus
{
    /// <summary />
    Draft,
    /// <summary />
    Scheduled,
    /// <summary />
    Sending,
    /// <summary />
    Sent,
    /// <summary />
    Paused,
    /// <summary />
    Cancelled,
    /// <summary />
    Failed
}