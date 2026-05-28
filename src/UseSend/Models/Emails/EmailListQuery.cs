namespace UseSend;

/// <summary>
///     Query parameters for <c>GET /v1/emails</c>.
/// </summary>
public class EmailListQuery
{
    /// <summary>Page number (1-based). Default: 1.</summary>
    public int? Page { get; set; }

    /// <summary>Results per page. Default: 50.</summary>
    public int? Limit { get; set; }

    /// <summary>Filter emails created on or after this date.</summary>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>Filter emails created on or before this date.</summary>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>Filter by domain ID (or multiple IDs).</summary>
    public object? DomainId { get; set; }
}