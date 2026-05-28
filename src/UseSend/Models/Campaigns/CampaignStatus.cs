using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Campaign lifecycle status.
/// </summary>
public enum CampaignStatus
{
    /// <summary />
    [JsonStringEnumMemberName("DRAFT")] Draft,

    /// <summary />
    [JsonStringEnumMemberName("SCHEDULED")]
    Scheduled,

    /// <summary />
    [JsonStringEnumMemberName("SENDING")] Sending,

    /// <summary />
    [JsonStringEnumMemberName("SENT")] Sent,

    /// <summary />
    [JsonStringEnumMemberName("PAUSED")] Paused,

    /// <summary />
    [JsonStringEnumMemberName("CANCELLED")]
    Cancelled,

    /// <summary />
    [JsonStringEnumMemberName("FAILED")] Failed
}