namespace UseSend;

/// <summary>
///     Response from <c>GET /v1/analytics/email-time-series</c>.
/// </summary>
public class EmailTimeSeriesResult
{
    /// <summary>Daily data points.</summary>
    public List<EmailTimeSeriesItem> Result { get; set; } = new();

    /// <summary>Aggregated totals across the queried period.</summary>
    public EmailMetricsTotals TotalCounts { get; set; } = new();
}