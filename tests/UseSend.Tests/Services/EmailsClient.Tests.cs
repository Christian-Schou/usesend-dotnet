namespace UseSend.Tests;

public class UseSendClientEmailTests
{
    [Fact]
    public async Task SendAsync_Success_ReturnsEmailId()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new { emailId = "abc123" });

        var result = await client.Emails.SendAsync(new EmailMessage
        {
            From = "from@example.com",
            To = "to@example.com",
            Subject = "Test",
            Text = "Hello"
        });

        Assert.True(result.Success);
        Assert.Equal("abc123", result.Content);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("v1/emails", handler.LastRequest.RequestUri!.ToString());
    }


    [Fact]
    public async Task SendAsync_WithIdempotencyKey_AddsHeader()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new { emailId = "idem123" });

        await client.Emails.SendAsync("my-key-123", new EmailMessage
        {
            From = "from@example.com",
            To = "to@example.com",
            Subject = "Idempotent"
        });

        Assert.True(handler.LastRequest!.Headers.Contains("Idempotency-Key"));
        Assert.Equal("my-key-123", handler.LastRequest.Headers.GetValues("Idempotency-Key").First());
    }


    [Fact]
    public async Task SendAsync_ApiError_ReturnsUnsuccessfulResponse()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.UnprocessableEntity, new { error = "Invalid from address" });

        var result = await client.Emails.SendAsync(new EmailMessage
        {
            From = "bad",
            To = "to@example.com",
            Subject = "Fail"
        });

        Assert.False(result.Success);
        Assert.NotNull(result.Exception);
        Assert.Equal(422, result.Exception.StatusCode);
    }


    [Fact]
    public async Task GetAsync_ReturnsReceipt()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            id = "abc123",
            teamId = 1,
            to = "to@example.com",
            from = "from@example.com",
            subject = "Test",
            html = (string?)null,
            text = "Hello",
            createdAt = DateTimeOffset.UtcNow.ToString("o"),
            updatedAt = DateTimeOffset.UtcNow.ToString("o"),
            emailEvents = Array.Empty<object>()
        });

        var result = await client.Emails.GetAsync("abc123");

        Assert.True(result.Success);
        Assert.Equal("abc123", result.Content.Id);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
    }


    [Fact]
    public async Task BatchAsync_SendsArrayAndReturnsIds()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            data = new[] { new { emailId = "e1" }, new { emailId = "e2" } }
        });

        var emails = new[]
        {
            new EmailMessage { From = "f@e.com", To = "a@e.com", Subject = "A" },
            new EmailMessage { From = "f@e.com", To = "b@e.com", Subject = "B" }
        };

        var result = await client.Emails.BatchAsync(emails);

        Assert.True(result.Success);
        Assert.Equal(2, result.Content.Count);
        Assert.Equal("e1", result.Content[0]);
        Assert.Equal("e2", result.Content[1]);
    }


    [Fact]
    public async Task CancelScheduleAsync_PostsToCorrectPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.Emails.CancelScheduleAsync("sched123");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("sched123/cancel", handler.LastRequest.RequestUri!.ToString());
    }


    [Fact]
    public async Task UpdateScheduleAsync_PatchesCorrectPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.Emails.UpdateScheduleAsync("sched456", DateTimeOffset.UtcNow.AddHours(1));

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Patch, handler.LastRequest!.Method);
        Assert.Contains("sched456", handler.LastRequest.RequestUri!.ToString());
    }
}