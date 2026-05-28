using UseSend;

var client = UseSendClient.Create(new UseSendClientOptions
{
    ApiToken = Environment.GetEnvironmentVariable("USESEND_API_KEY") ?? "us_your_api_token",
    ApiUrl = "https://send.mycompany.com/api/" // your self-hosted useSend URL
});

// List domains accessible via this instance
var domainsResponse = await client.Domains.ListAsync();

if (domainsResponse.Success)
{
    Console.WriteLine($"Found {domainsResponse.Content.Count} domain(s):");

    foreach (var domain in domainsResponse.Content)
        Console.WriteLine($"  [{domain.Status}] {domain.Name}");
}
else
{
    Console.WriteLine($"Error: {domainsResponse.Exception?.Message}");
}