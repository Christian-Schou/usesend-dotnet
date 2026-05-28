namespace UseSend.Tests;

public class UseSendClientContactTests
{
    private const string BookId = "book_abc";

    [Fact]
    public async Task CreateAsync_ReturnsContactId()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new { contactId = "con_1" });

        var result = await client.Contacts.CreateAsync(BookId, new ContactData { Email = "user@example.com" });

        Assert.True(result.Success);
        Assert.Equal("con_1", result.Content);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains(BookId, handler.LastRequest.RequestUri!.ToString());
    }


    [Fact]
    public async Task GetAsync_ReturnsContact()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new
        {
            id = "con_1", email = "user@example.com", properties = new { },
            contactBookId = BookId, createdAt = DateTimeOffset.UtcNow.ToString("o"),
            updatedAt = DateTimeOffset.UtcNow.ToString("o")
        });

        var result = await client.Contacts.GetAsync(BookId, "con_1");

        Assert.True(result.Success);
        Assert.Equal("user@example.com", result.Content.Email);
    }


    [Fact]
    public async Task UpdateAsync_PatchesCorrectPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.Contacts.UpdateAsync(BookId, "con_1",
            new ContactUpdateData { FirstName = "Alice" });

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Patch, handler.LastRequest!.Method);
    }


    [Fact]
    public async Task DeleteAsync_DeletesCorrectPath()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK);

        var result = await client.Contacts.DeleteAsync(BookId, "con_1");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
        Assert.Contains("con_1", handler.LastRequest.RequestUri!.ToString());
    }


    [Fact]
    public async Task BulkCreateAsync_ReturnsCount()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new { message = "ok", count = 3 });

        var result = await client.Contacts.BulkCreateAsync(BookId, new[]
        {
            new ContactData { Email = "a@e.com" },
            new ContactData { Email = "b@e.com" },
            new ContactData { Email = "c@e.com" }
        });

        Assert.True(result.Success);
        Assert.Equal(3, result.Content.Count);
    }


    [Fact]
    public async Task BulkDeleteAsync_ReturnsResult()
    {
        var (client, handler) = ClientFactory.Create();
        handler.SetResponse(HttpStatusCode.OK, new { success = true, count = 2 });

        var result = await client.Contacts.BulkDeleteAsync(BookId,
            new BulkDeleteData { ContactIds = ["con_1", "con_2"] });

        Assert.True(result.Success);
        Assert.True(result.Content.Success);
        Assert.Equal(2, result.Content.Count);
    }
}