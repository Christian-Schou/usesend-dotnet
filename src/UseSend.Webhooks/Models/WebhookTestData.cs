using System.Text.Json.Serialization;

namespace UseSend.Webhooks;

/// <summary>
///     Data payload for <c>webhook.test</c> events sent from the useSend dashboard.
/// </summary>
public sealed class WebhookTestData
{
    /// <summary>Always <c>true</c> for test events.</summary>
    [JsonPropertyName("test")]
    public bool Test { get; init; }

    /// <summary>The webhook ID that sent the test.</summary>
    [JsonPropertyName("webhookId")]
    public string WebhookId { get; init; } = string.Empty;

    /// <summary>When the test was sent.</summary>
    [JsonPropertyName("sentAt")]
    public DateTimeOffset SentAt { get; init; }
}