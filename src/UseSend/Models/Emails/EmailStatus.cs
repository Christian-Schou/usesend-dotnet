using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Email status values returned by the useSend API.
/// </summary>
public enum EmailStatus
{
    /// <summary />
    [JsonStringEnumMemberName("SCHEDULED")]
    Scheduled,

    /// <summary />
    [JsonStringEnumMemberName("QUEUED")] Queued,

    /// <summary />
    [JsonStringEnumMemberName("SENT")] Sent,

    /// <summary />
    [JsonStringEnumMemberName("DELIVERY_DELAYED")]
    DeliveryDelayed,

    /// <summary />
    [JsonStringEnumMemberName("BOUNCED")] Bounced,

    /// <summary />
    [JsonStringEnumMemberName("REJECTED")] Rejected,

    /// <summary />
    [JsonStringEnumMemberName("RENDERING_FAILURE")]
    RenderingFailure,

    /// <summary />
    [JsonStringEnumMemberName("DELIVERED")]
    Delivered,

    /// <summary />
    [JsonStringEnumMemberName("OPENED")] Opened,

    /// <summary />
    [JsonStringEnumMemberName("CLICKED")] Clicked,

    /// <summary />
    [JsonStringEnumMemberName("COMPLAINED")]
    Complained,

    /// <summary />
    [JsonStringEnumMemberName("FAILED")] Failed,

    /// <summary />
    [JsonStringEnumMemberName("CANCELLED")]
    Cancelled,

    /// <summary />
    [JsonStringEnumMemberName("SUPPRESSED")]
    Suppressed
}