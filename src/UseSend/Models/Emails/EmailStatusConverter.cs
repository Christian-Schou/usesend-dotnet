using System.Text.Json;
using System.Text.Json.Serialization;

namespace UseSend;

internal sealed class EmailStatusConverter : JsonConverter<EmailStatus>
{
    private static readonly Dictionary<string, EmailStatus> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SCHEDULED"]          = EmailStatus.Scheduled,
        ["QUEUED"]             = EmailStatus.Queued,
        ["SENT"]               = EmailStatus.Sent,
        ["DELIVERY_DELAYED"]   = EmailStatus.DeliveryDelayed,
        ["BOUNCED"]            = EmailStatus.Bounced,
        ["REJECTED"]           = EmailStatus.Rejected,
        ["RENDERING_FAILURE"]  = EmailStatus.RenderingFailure,
        ["DELIVERED"]          = EmailStatus.Delivered,
        ["OPENED"]             = EmailStatus.Opened,
        ["CLICKED"]            = EmailStatus.Clicked,
        ["COMPLAINED"]         = EmailStatus.Complained,
        ["FAILED"]             = EmailStatus.Failed,
        ["CANCELLED"]          = EmailStatus.Cancelled,
        ["SUPPRESSED"]         = EmailStatus.Suppressed,
    };

    private static readonly Dictionary<EmailStatus, string> _reverseMap =
        _map.ToDictionary(kv => kv.Value, kv => kv.Key);

    public override EmailStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is not null && _map.TryGetValue(value, out var status))
            return status;

        throw new JsonException($"Unknown EmailStatus value: '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, EmailStatus value, JsonSerializerOptions options)
        => writer.WriteStringValue(_reverseMap.TryGetValue(value, out var s) ? s : value.ToString().ToUpperInvariant());
}
