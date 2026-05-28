using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     DNS record type.
/// </summary>
public enum DnsRecordType
{
    /// <summary />
    [JsonStringEnumMemberName("MX")] Mx,

    /// <summary />
    [JsonStringEnumMemberName("TXT")] Txt
}