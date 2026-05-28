using System.Text.Json;
using System.Text.Json.Serialization;

namespace UseSend;

internal sealed class DomainStatusConverter : JsonConverter<DomainStatus>
{
    private static readonly Dictionary<string, DomainStatus> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["NOT_STARTED"]       = DomainStatus.NotStarted,
        ["PENDING"]           = DomainStatus.Pending,
        ["SUCCESS"]           = DomainStatus.Success,
        ["FAILED"]            = DomainStatus.Failed,
        ["TEMPORARY_FAILURE"] = DomainStatus.TemporaryFailure,
    };

    private static readonly Dictionary<DomainStatus, string> _reverseMap =
        _map.ToDictionary(kv => kv.Value, kv => kv.Key);

    public override DomainStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is not null && _map.TryGetValue(value, out var status))
            return status;
        throw new JsonException($"Unknown DomainStatus value: '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, DomainStatus value, JsonSerializerOptions options)
        => writer.WriteStringValue(_reverseMap.TryGetValue(value, out var s) ? s : value.ToString().ToUpperInvariant());
}

internal sealed class DnsRecordTypeConverter : JsonConverter<DnsRecordType>
{
    private static readonly Dictionary<string, DnsRecordType> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["MX"]  = DnsRecordType.Mx,
        ["TXT"] = DnsRecordType.Txt,
    };

    private static readonly Dictionary<DnsRecordType, string> _reverseMap =
        _map.ToDictionary(kv => kv.Value, kv => kv.Key);

    public override DnsRecordType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is not null && _map.TryGetValue(value, out var type))
            return type;
        throw new JsonException($"Unknown DnsRecordType value: '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, DnsRecordType value, JsonSerializerOptions options)
        => writer.WriteStringValue(_reverseMap.TryGetValue(value, out var s) ? s : value.ToString().ToUpperInvariant());
}

internal sealed class CampaignStatusConverter : JsonConverter<CampaignStatus>
{
    private static readonly Dictionary<string, CampaignStatus> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["DRAFT"]     = CampaignStatus.Draft,
        ["SCHEDULED"] = CampaignStatus.Scheduled,
        ["SENDING"]   = CampaignStatus.Sending,
        ["SENT"]      = CampaignStatus.Sent,
        ["PAUSED"]    = CampaignStatus.Paused,
        ["CANCELLED"] = CampaignStatus.Cancelled,
        ["FAILED"]    = CampaignStatus.Failed,
    };

    private static readonly Dictionary<CampaignStatus, string> _reverseMap =
        _map.ToDictionary(kv => kv.Value, kv => kv.Key);

    public override CampaignStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is not null && _map.TryGetValue(value, out var status))
            return status;
        throw new JsonException($"Unknown CampaignStatus value: '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, CampaignStatus value, JsonSerializerOptions options)
        => writer.WriteStringValue(_reverseMap.TryGetValue(value, out var s) ? s : value.ToString().ToUpperInvariant());
}
