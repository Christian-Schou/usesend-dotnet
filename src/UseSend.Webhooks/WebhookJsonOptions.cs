using System.Text.Json;

namespace UseSend.Webhooks;

internal static class WebhookJsonOptions
{
    internal static readonly JsonSerializerOptions Default = new()
    {
        PropertyNameCaseInsensitive = true
    };
}