namespace UseSend.Tests;

public class UseSendClientAnalyticsTests
{
    [Fact]
    public async Task EmailTimeSeriesAsync_ReturnsResult()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            result = new[]
            {
                new
                {
                    date = "2026-05-01", sent = 100, delivered = 95, opened = 40, clicked = 10, bounced = 2,
                    complained = 0
                }
            },
            totalCounts = new { sent = 100, delivered = 95, opened = 40, clicked = 10, bounced = 2, complained = 0 }
        });

        var result = await client.Analytics.EmailTimeSeriesAsync();

        Assert.True(result.Success);
        Assert.Single(result.Content.Result);
        Assert.Equal(100, result.Content.TotalCounts.Sent);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("email-time-series", handler.LastRequest.RequestUri!.ToString());
    }


    [Fact]
    public async Task EmailTimeSeriesAsync_WithQuery_AppendsQueryString()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            result = Array.Empty<object>(),
            totalCounts = new { sent = 0, delivered = 0, opened = 0, clicked = 0, bounced = 0, complained = 0 }
        });

        await client.Analytics.EmailTimeSeriesAsync(new AnalyticsQuery { Days = 7, DomainId = "123" });

        var uri = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("days=7", uri);
        Assert.Contains("domainId=123", uri);
    }


    [Fact]
    public async Task ReputationMetricsAsync_ReturnsResult()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            reputationScore = 98.5,
            bounceRate = 0.02,
            complaintRate = 0.001
        });

        var result = await client.Analytics.ReputationMetricsAsync();

        Assert.True(result.Success);
        Assert.Contains("reputation-metrics", handler.LastRequest!.RequestUri!.ToString());
    }
}