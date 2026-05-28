namespace UseSend;

/// <summary>
///     Request body for creating a domain.
/// </summary>
public class DomainCreateData
{
    /// <summary>Domain name (e.g. <c>example.com</c>). Required.</summary>
    public string Name { get; set; } = default!;

    /// <summary>AWS region (e.g. <c>us-east-1</c>). Required.</summary>
    public string Region { get; set; } = default!;
}