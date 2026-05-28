namespace UseSend.Tests;

public class UseSendClientContactBookTests
{
    [Fact]
    public async Task ListAsync_ReturnsList()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new[]
        {
            new
            {
                id = "cb1", name = "Newsletter", teamId = 1L,
                properties = new { }, variables = Array.Empty<string>(), emoji = "📧",
                createdAt = DateTimeOffset.UtcNow.ToString("o"), updatedAt = DateTimeOffset.UtcNow.ToString("o")
            }
        });

        var result = await client.ContactBooks.ListAsync();

        Assert.True(result.Success);
        Assert.Single(result.Content);
        Assert.Equal("cb1", result.Content[0].Id);
    }


    [Fact]
    public async Task CreateAsync_PostsAndReturnsBook()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            id = "cb_new", name = "My List", teamId = 1L,
            properties = new { }, variables = Array.Empty<string>(), emoji = "📋",
            createdAt = DateTimeOffset.UtcNow.ToString("o"), updatedAt = DateTimeOffset.UtcNow.ToString("o")
        });

        var result = await client.ContactBooks.CreateAsync(new ContactBookData { Name = "My List" });

        Assert.True(result.Success);
        Assert.Equal("cb_new", result.Content.Id);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }


    [Fact]
    public async Task DeleteAsync_DeletesCorrectPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.ContactBooks.DeleteAsync("cb1");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
        Assert.Contains("cb1", handler.LastRequest.RequestUri!.ToString());
    }
}