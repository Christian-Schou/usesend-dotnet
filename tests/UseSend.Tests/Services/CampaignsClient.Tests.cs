namespace UseSend.Tests;

public class UseSendClientCampaignTests
{
    private static object MakeCampaign(string id, string status = "DRAFT")
    {
        return new
        {
            id,
            name = "Test Campaign",
            from = "from@example.com",
            subject = "Newsletter",
            previewText = (string?)null,
            contactBookId = "cb1",
            html = (string?)null,
            content = (string?)null,
            status,
            scheduledAt = (string?)null,
            batchSize = 1000,
            batchWindowMinutes = 60,
            total = 0, sent = 0, delivered = 0, opened = 0, clicked = 0,
            unsubscribed = 0, bounced = 0, hardBounced = 0, complained = 0,
            replyTo = Array.Empty<string>(),
            cc = Array.Empty<string>(),
            bcc = Array.Empty<string>(),
            createdAt = DateTimeOffset.UtcNow.ToString("o"),
            updatedAt = DateTimeOffset.UtcNow.ToString("o")
        };
    }


    [Fact]
    public async Task CreateAsync_PostsAndReturnsCampaign()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, MakeCampaign("camp_1"));

        var result = await client.Campaigns.CreateAsync(new CampaignCreateData
        {
            Name = "Test Campaign",
            From = "from@example.com",
            Subject = "Newsletter",
            ContactBookId = "cb1"
        });

        Assert.True(result.Success);
        Assert.Equal("camp_1", result.Content.Id);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }


    [Fact]
    public async Task ScheduleAsync_PostsToSchedulePath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, MakeCampaign("camp_1", "SCHEDULED"));

        var result = await client.Campaigns.ScheduleAsync("camp_1",
            new CampaignScheduleData { ScheduledAt = "2026-06-01T09:00:00Z" });

        Assert.True(result.Success);
        Assert.Contains("camp_1/schedule", handler.LastRequest!.RequestUri!.ToString());
    }


    [Fact]
    public async Task PauseAsync_PostsToPausePath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, MakeCampaign("camp_1", "PAUSED"));

        var result = await client.Campaigns.PauseAsync("camp_1");

        Assert.True(result.Success);
        Assert.Contains("camp_1/pause", handler.LastRequest!.RequestUri!.ToString());
    }


    [Fact]
    public async Task ResumeAsync_PostsToResumePath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, MakeCampaign("camp_1", "SENDING"));

        var result = await client.Campaigns.ResumeAsync("camp_1");

        Assert.True(result.Success);
        Assert.Contains("camp_1/resume", handler.LastRequest!.RequestUri!.ToString());
    }


    [Fact]
    public async Task DeleteAsync_DeletesCorrectPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.Campaigns.DeleteAsync("camp_1");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
    }
}