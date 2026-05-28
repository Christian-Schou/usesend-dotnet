using Microsoft.AspNetCore.WebUtilities;

namespace UseSend;

/// <summary>
///     Operations on the <c>/v1/analytics</c> resource.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>Retrieves email sending metrics as a time series (daily breakdown).</summary>
    Task<UseSendResponse<EmailTimeSeriesResult>> EmailTimeSeriesAsync(AnalyticsQuery? query = null,
        CancellationToken cancellationToken = default);

    /// <summary>Retrieves sender reputation metrics.</summary>
    Task<UseSendResponse<ReputationMetricsResult>> ReputationMetricsAsync(AnalyticsQuery? query = null,
        CancellationToken cancellationToken = default);
}

internal sealed class AnalyticsService : IAnalyticsService
{
    private readonly HttpExecutor _x;

    internal AnalyticsService(HttpExecutor executor)
    {
        _x = executor;
    }


    /// <inheritdoc />
    public Task<UseSendResponse<EmailTimeSeriesResult>> EmailTimeSeriesAsync(AnalyticsQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, BuildUrl("v1/analytics/email-time-series", query));

        return _x.Execute<EmailTimeSeriesResult, EmailTimeSeriesResult>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<ReputationMetricsResult>> ReputationMetricsAsync(AnalyticsQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, BuildUrl("v1/analytics/reputation-metrics", query));

        return _x.Execute<ReputationMetricsResult, ReputationMetricsResult>(req, x => x, cancellationToken);
    }


    private static string BuildUrl(string baseUrl, AnalyticsQuery? query)
    {
        if (query == null)
            return baseUrl;

        var qs = new Dictionary<string, string?>();

        if (query.Days.HasValue)
            qs.Add("days", query.Days.Value.ToString());

        if (query.DomainId != null)
            qs.Add("domainId", query.DomainId);

        return qs.Count > 0 ? QueryHelpers.AddQueryString(baseUrl, qs) : baseUrl;
    }
}