namespace UseSend;

/// <summary>
///     Query parameters for analytics endpoints.
/// </summary>
public class AnalyticsQuery
{
    /// <summary>Number of days to retrieve data for. Allowed: 7 or 30. Default: 30.</summary>
    public int? Days { get; set; }

    /// <summary>Filter by domain ID.</summary>
    public string? DomainId { get; set; }
}