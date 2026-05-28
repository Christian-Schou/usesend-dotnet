using System.Text.Json;
using System.Text.Json.Serialization;

namespace UseSend.Webhooks;

/// <summary>
///     Represents a webhook event received from useSend.
/// </summary>
public sealed class WebhookEvent
{
    /// <summary>Unique identifier for this webhook call.</summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>The event type (e.g. <c>email.delivered</c>). See <see cref="WebhookEventType" />.</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>API version for the payload format.</summary>
    [JsonPropertyName("version")]
    public string Version { get; init; } = string.Empty;

    /// <summary>When the event was created.</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Your team ID.</summary>
    [JsonPropertyName("teamId")]
    public long TeamId { get; init; }

    /// <summary>Delivery attempt number (1–6).</summary>
    [JsonPropertyName("attempt")]
    public int Attempt { get; init; }

    /// <summary>
    ///     Raw event-specific data. Use <see cref="GetData{T}" /> to deserialize to a typed model.
    /// </summary>
    [JsonPropertyName("data")]
    public JsonElement Data { get; init; }

    /// <summary>
    ///     Deserializes <see cref="Data" /> to the specified type.
    /// </summary>
    public T? GetData<T>(JsonSerializerOptions? options = null)
    {
        return Data.Deserialize<T>(options ?? WebhookJsonOptions.Default);
    }
}