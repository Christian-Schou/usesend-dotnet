using System.Text.Json;
using System.Text.Json.Serialization;

namespace UseSend.Webhooks;

/// <summary>
///     Base data payload shared by all email webhook events.
/// </summary>
public class EmailEventData
{
    /// <summary>Email ID.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>Email status (e.g. <c>DELIVERED</c>, <c>BOUNCED</c>).</summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>Sender email address.</summary>
    [JsonPropertyName("from")]
    public string From { get; init; } = string.Empty;

    /// <summary>Recipient email addresses.</summary>
    [JsonPropertyName("to")]
    public List<string> To { get; init; } = [];

    /// <summary>When this specific event occurred.</summary>
    [JsonPropertyName("occurredAt")]
    public DateTimeOffset OccurredAt { get; init; }

    /// <summary>Email subject.</summary>
    [JsonPropertyName("subject")]
    public string? Subject { get; init; }

    /// <summary>Campaign ID if the email was sent from a campaign.</summary>
    [JsonPropertyName("campaignId")]
    public string? CampaignId { get; init; }

    /// <summary>Contact ID if the email was sent to a contact.</summary>
    [JsonPropertyName("contactId")]
    public string? ContactId { get; init; }

    /// <summary>Domain ID.</summary>
    [JsonPropertyName("domainId")]
    public long? DomainId { get; init; }

    /// <summary>Template ID if a template was used.</summary>
    [JsonPropertyName("templateId")]
    public string? TemplateId { get; init; }

    /// <summary>Custom metadata attached to the email.</summary>
    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; init; }
}

/// <summary>Data payload for <c>email.bounced</c> events.</summary>
public sealed class EmailBouncedEventData : EmailEventData
{
    /// <summary>Bounce details.</summary>
    [JsonPropertyName("bounce")]
    public BounceDetails? Bounce { get; init; }
}

/// <summary>Details of an email bounce.</summary>
public sealed class BounceDetails
{
    /// <summary>Bounce type: <c>Transient</c>, <c>Permanent</c>, or <c>Undetermined</c>.</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>Bounce sub-type.</summary>
    [JsonPropertyName("subType")]
    public string SubType { get; init; } = string.Empty;

    /// <summary>Bounce message from the mail server.</summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}

/// <summary>Data payload for <c>email.failed</c> events.</summary>
public sealed class EmailFailedEventData : EmailEventData
{
    /// <summary>Failure details.</summary>
    [JsonPropertyName("failed")]
    public FailureDetails? Failed { get; init; }
}

/// <summary>Details of an email failure.</summary>
public sealed class FailureDetails
{
    /// <summary>Reason the email failed.</summary>
    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;
}

/// <summary>Data payload for <c>email.suppressed</c> events.</summary>
public sealed class EmailSuppressedEventData : EmailEventData
{
    /// <summary>Suppression details.</summary>
    [JsonPropertyName("suppression")]
    public SuppressionDetails? Suppression { get; init; }
}

/// <summary>Details of an email suppression.</summary>
public sealed class SuppressionDetails
{
    /// <summary>Suppression type: <c>Bounce</c>, <c>Complaint</c>, or <c>Manual</c>.</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>Why the email was suppressed.</summary>
    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;

    /// <summary>Source of the suppression.</summary>
    [JsonPropertyName("source")]
    public string? Source { get; init; }
}

/// <summary>Data payload for <c>email.opened</c> events.</summary>
public sealed class EmailOpenedEventData : EmailEventData
{
    /// <summary>Open tracking details.</summary>
    [JsonPropertyName("open")]
    public OpenDetails? Open { get; init; }
}

/// <summary>Details of an email open event.</summary>
public sealed class OpenDetails
{
    /// <summary>When the email was opened.</summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>Browser/client user agent.</summary>
    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; init; }

    /// <summary>IP address of the opener.</summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; init; }

    /// <summary>Detected platform.</summary>
    [JsonPropertyName("platform")]
    public string? Platform { get; init; }
}

/// <summary>Data payload for <c>email.clicked</c> events.</summary>
public sealed class EmailClickedEventData : EmailEventData
{
    /// <summary>Click tracking details.</summary>
    [JsonPropertyName("click")]
    public ClickDetails? Click { get; init; }
}

/// <summary>Details of a link click event.</summary>
public sealed class ClickDetails
{
    /// <summary>When the link was clicked.</summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>The URL that was clicked.</summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    /// <summary>Browser/client user agent.</summary>
    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; init; }

    /// <summary>IP address of the clicker.</summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; init; }

    /// <summary>Detected platform.</summary>
    [JsonPropertyName("platform")]
    public string? Platform { get; init; }
}