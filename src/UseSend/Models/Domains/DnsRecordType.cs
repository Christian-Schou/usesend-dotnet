using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     DNS record type.
/// </summary>
[JsonConverter(typeof(DnsRecordTypeConverter))]
public enum DnsRecordType
{
    /// <summary />
    Mx,
    /// <summary />
    Txt
}