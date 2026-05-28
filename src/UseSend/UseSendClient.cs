using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace UseSend;

/// <summary>
///     useSend client. Holds service instances for each API resource group.
/// </summary>
public class UseSendClient : IUseSend
{
    /// <summary>
    ///     Initializes a new instance of <see cref="UseSendClient" />.
    /// </summary>
    /// <param name="options">Configuration options.</param>
    /// <param name="httpClient">HTTP client instance (injected by IHttpClientFactory).</param>
    public UseSendClient(IOptionsSnapshot<UseSendClientOptions> options, HttpClient httpClient)
    {
        var opt = options.Value;

        var apiToken = opt.ApiToken;

        if (string.IsNullOrWhiteSpace(apiToken))
            apiToken = Environment.GetEnvironmentVariable("USESEND_API_KEY");

        if (string.IsNullOrWhiteSpace(apiToken))
            throw new InvalidOperationException(
                "useSend API token is required. Set UseSendClientOptions.ApiToken or the USESEND_API_KEY environment variable.");

        httpClient.BaseAddress = new Uri(opt.ApiUrl.TrimEnd('/') + "/");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var sdkVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("usesend-dotnet", sdkVersion));
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("dotnet",
            Environment.Version.ToString()));

        var executor = new HttpExecutor(httpClient, opt.ThrowExceptions);

        Emails = new EmailService(executor);
        Domains = new DomainService(executor);
        Contacts = new ContactService(executor);
        ContactBooks = new ContactBookService(executor);
        Campaigns = new CampaignService(executor);
        Analytics = new AnalyticsService(executor);
    }

    /// <inheritdoc />
    public IEmailService Emails { get; }

    /// <inheritdoc />
    public IDomainService Domains { get; }

    /// <inheritdoc />
    public IContactService Contacts { get; }

    /// <inheritdoc />
    public IContactBookService ContactBooks { get; }

    /// <inheritdoc />
    public ICampaignService Campaigns { get; }

    /// <inheritdoc />
    public IAnalyticsService Analytics { get; }


    /// <summary>
    ///     Creates a standalone <see cref="UseSendClient" /> without dependency injection.
    ///     Uses the official useSend cloud endpoint.
    /// </summary>
    /// <param name="apiToken">Your useSend API token.</param>
    public static IUseSend Create(string apiToken)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(apiToken);

        return Create(new UseSendClientOptions { ApiToken = apiToken });
    }


    /// <summary>
    ///     Creates a standalone <see cref="UseSendClient" /> without dependency injection.
    ///     Falls back to the <c>USESEND_API_KEY</c> environment variable if no token is set.
    /// </summary>
    public static IUseSend Create()
    {
        return Create(new UseSendClientOptions());
    }


    /// <summary>
    ///     Creates a standalone <see cref="UseSendClient" /> without dependency injection.
    /// </summary>
    /// <param name="options">Configuration options.</param>
    public static IUseSend Create(UseSendClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var httpClient = new HttpClient();
        return new UseSendClient(new OptionsSnapshot<UseSendClientOptions>(options), httpClient);
    }


    /// <summary>
    ///     Creates a standalone <see cref="UseSendClient" /> with a custom <see cref="HttpClient" />.
    ///     Intended for testing purposes.
    /// </summary>
    internal static IUseSend Create(UseSendClientOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);

        return new UseSendClient(new OptionsSnapshot<UseSendClientOptions>(options), httpClient);
    }
}