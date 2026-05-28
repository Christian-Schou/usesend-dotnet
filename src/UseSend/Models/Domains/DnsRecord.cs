namespace UseSend;

/// <summary>
///     A DNS record required for domain verification.
/// </summary>
public class DnsRecord
{
    /// <summary>Record type (MX or TXT).</summary>
    public DnsRecordType Type { get; set; }

    /// <summary>Record name.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Record value.</summary>
    public string Value { get; set; } = default!;

    /// <summary>TTL (e.g. "Auto").</summary>
    public string Ttl { get; set; } = default!;

    /// <summary>Priority (MX records only).</summary>
    public string? Priority { get; set; }

    /// <summary>Current verification status of this record.</summary>
    public DomainStatus Status { get; set; }

    /// <summary>Whether this record is recommended.</summary>
    public bool? Recommended { get; set; }
}