using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Email status values returned by the useSend API.
/// </summary>
[JsonConverter(typeof(EmailStatusConverter))]
public enum EmailStatus
{
    /// <summary />
    Scheduled,
    /// <summary />
    Queued,
    /// <summary />
    Sent,
    /// <summary />
    DeliveryDelayed,
    /// <summary />
    Bounced,
    /// <summary />
    Rejected,
    /// <summary />
    RenderingFailure,
    /// <summary />
    Delivered,
    /// <summary />
    Opened,
    /// <summary />
    Clicked,
    /// <summary />
    Complained,
    /// <summary />
    Failed,
    /// <summary />
    Cancelled,
    /// <summary />
    Suppressed
}