using System.Text;

namespace UseSend.Tests;

/// <summary>
///     Builds a UseSendClient backed by a fake HttpMessageHandler for unit testing.
/// </summary>
internal static class ClientFactory
{
    internal static (IUseSend client, FakeHandler handler) Create(
        string apiToken = "us_test_123",
        string apiUrl = "https://app.usesend.com/api/")
    {
        var handler = new FakeHandler();
        var httpClient = new HttpClient(handler);

        var options = new UseSendClientOptions
        {
            ApiToken = apiToken,
            ApiUrl = apiUrl,
            ThrowExceptions = false
        };

        var client = UseSendClient.Create(options, httpClient);
        return (client, handler);
    }
}

/// <summary>
///     A fake HttpMessageHandler that returns a preset response.
/// </summary>
internal sealed class FakeHandler : HttpMessageHandler
{
    private HttpResponseMessage _response = new(HttpStatusCode.OK);

    internal HttpRequestMessage? LastRequest { get; private set; }

    internal void SetResponse(HttpStatusCode status, object? body = null)
    {
        _response = new HttpResponseMessage(status);

        if (body != null)
            _response.Content = JsonContent.Create(body);
    }

    internal void SetResponse(HttpStatusCode status, string body)
    {
        _response = new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequest = request;
        return Task.FromResult(_response);
    }
}