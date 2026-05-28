using System.Text.Json.Serialization;

namespace UseSend.Webhooks;

/// <summary>
///     Data payload for all domain webhook events
///     (<c>domain.created</c>, <c>domain.verified</c>, <c>domain.updated</c>, <c>domain.deleted</c>).
/// </summary>
public sealed class DomainEventData
{
    /// <summary>Domain ID.</summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }

    /// <summary>Domain name (e.g. <c>example.com</c>).</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>Current domain status.</summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>AWS region the domain is configured in.</summary>
    [JsonPropertyName("region")]
    public string Region { get; init; } = string.Empty;

    /// <summary>Whether click tracking is enabled.</summary>
    [JsonPropertyName("clickTracking")]
    public bool ClickTracking { get; init; }

    /// <summary>Whether open tracking is enabled.</summary>
    [JsonPropertyName("openTracking")]
    public bool OpenTracking { get; init; }

    /// <summary>When the domain was created.</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>When the domain was last updated.</summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>Subdomain used for tracking links.</summary>
    [JsonPropertyName("subdomain")]
    public string? Subdomain { get; init; }

    /// <summary>DKIM verification status.</summary>
    [JsonPropertyName("dkimStatus")]
    public string? DkimStatus { get; init; }

    /// <summary>SPF record details.</summary>
    [JsonPropertyName("spfDetails")]
    public string? SpfDetails { get; init; }

    /// <summary>Whether a DMARC record has been added.</summary>
    [JsonPropertyName("dmarcAdded")]
    public bool? DmarcAdded { get; init; }
}