namespace UseSend.Tests;

public class UseSendClientDomainTests
{
    [Fact]
    public async Task ListAsync_ReturnsListOfDomains()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new[]
        {
            new
            {
                id = 1L, name = "example.com", teamId = 1L, status = "SUCCESS",
                publicKey = "key", createdAt = DateTimeOffset.UtcNow.ToString("o"),
                updatedAt = DateTimeOffset.UtcNow.ToString("o"), dnsRecords = Array.Empty<object>()
            }
        });

        var result = await client.Domains.ListAsync();

        Assert.True(result.Success);
        Assert.Single(result.Content);
        Assert.Equal("example.com", result.Content[0].Name);
    }


    [Fact]
    public async Task CreateAsync_PostsAndReturnsDomain()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            id = 42L, name = "newdomain.com", teamId = 1L, status = "NOT_STARTED",
            publicKey = "pk", createdAt = DateTimeOffset.UtcNow.ToString("o"),
            updatedAt = DateTimeOffset.UtcNow.ToString("o"), dnsRecords = Array.Empty<object>()
        });

        var result = await client.Domains.CreateAsync(new DomainCreateData
        {
            Name = "newdomain.com",
            Region = "us-east-1"
        });

        Assert.True(result.Success);
        Assert.Equal(42L, result.Content.Id);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }


    [Fact]
    public async Task DeleteAsync_ReturnsDeleteResult()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new { id = 1L, success = true, message = "deleted" });

        var result = await client.Domains.DeleteAsync(1L);

        Assert.True(result.Success);
        Assert.True(result.Content.Success);
        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
    }


    [Fact]
    public async Task VerifyAsync_PutsToVerifyPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.Domains.VerifyAsync(5L);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Put, handler.LastRequest!.Method);
        Assert.Contains("5/verify", handler.LastRequest.RequestUri!.ToString());
    }
}