namespace UseSend;

/// <summary>
///     A domain registered in useSend.
/// </summary>
public class Domain
{
    /// <summary>Domain identifier.</summary>
    public long Id { get; set; }

    /// <summary>Domain name (e.g. <c>example.com</c>).</summary>
    public string Name { get; set; } = default!;

    /// <summary>Team identifier.</summary>
    public long TeamId { get; set; }

    /// <summary>Verification status.</summary>
    public DomainStatus Status { get; set; }

    /// <summary>AWS region (e.g. <c>us-east-1</c>).</summary>
    public string? Region { get; set; }

    /// <summary>Whether click tracking is enabled.</summary>
    public bool ClickTracking { get; set; }

    /// <summary>Whether open tracking is enabled.</summary>
    public bool OpenTracking { get; set; }

    /// <summary>DKIM public key.</summary>
    public string? PublicKey { get; set; }

    /// <summary>DKIM status.</summary>
    public string? DkimStatus { get; set; }

    /// <summary>SPF details.</summary>
    public string? SpfDetails { get; set; }

    /// <summary>Whether DMARC record has been added.</summary>
    public bool DmarcAdded { get; set; }

    /// <summary>Whether verification is currently in progress.</summary>
    public bool IsVerifying { get; set; }

    /// <summary>Subdomain (if any).</summary>
    public string? Subdomain { get; set; }

    /// <summary>Error message from last verification attempt.</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>Verification error details.</summary>
    public string? VerificationError { get; set; }

    /// <summary>Last time the domain was checked.</summary>
    public DateTimeOffset? LastCheckedTime { get; set; }

    /// <summary>DNS records required for verification.</summary>
    public List<DnsRecord> DnsRecords { get; set; } = new();

    /// <summary>When the domain was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>When the domain was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}