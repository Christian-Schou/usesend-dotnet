namespace UseSend;

/// <summary>
///     Response from <c>GET /v1/analytics/reputation-metrics</c>.
/// </summary>
public class ReputationMetricsResult
{
    /// <summary>Sending reputation score (0–100, if provided).</summary>
    public double? ReputationScore { get; set; }

    /// <summary>Bounce rate percentage.</summary>
    public double? BounceRate { get; set; }

    /// <summary>Complaint rate percentage.</summary>
    public double? ComplaintRate { get; set; }

    /// <summary>Additional metrics returned by the API.</summary>
    public Dictionary<string, object?>? AdditionalData { get; set; }
}