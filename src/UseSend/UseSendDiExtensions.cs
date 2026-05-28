using Microsoft.Extensions.DependencyInjection;

namespace UseSend;

/// <summary>
///     Extension methods to register the useSend client for dependency injection.
/// </summary>
public static class UseSendDiExtensions
{
    /// <summary>
    ///     Registers <see cref="IUseSend" /> and all service interfaces for dependency injection
    ///     using the provided API token. The official useSend cloud endpoint is used.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="apiToken">Your useSend API token (e.g. <c>us_...</c>).</param>
    /// <returns>The <see cref="IHttpClientBuilder" /> for further configuration.</returns>
    public static IHttpClientBuilder AddUseSend(this IServiceCollection services, string apiToken)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(apiToken);

        return services.AddUseSend(options => { options.ApiToken = apiToken; });
    }

    /// <summary>
    ///     Registers <see cref="IUseSend" /> and all service interfaces for dependency injection
    ///     using a configuration delegate. Use this overload to specify a custom
    ///     <see cref="UseSendClientOptions.ApiUrl" /> for self-hosted useSend instances.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Delegate to configure <see cref="UseSendClientOptions" />.</param>
    /// <returns>The <see cref="IHttpClientBuilder" /> for further configuration.</returns>
    public static IHttpClientBuilder AddUseSend(this IServiceCollection services,
        Action<UseSendClientOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.Configure(configureOptions);

        // Register individual service interfaces as forwarding factories so callers can
        // inject e.g. IEmailService directly without going through IUseSend.
        services.AddScoped<IEmailService>(sp => (IEmailService)sp.GetRequiredService<IUseSend>());
        services.AddScoped<IDomainService>(sp => (IDomainService)sp.GetRequiredService<IUseSend>());
        services.AddScoped<IContactService>(sp => (IContactService)sp.GetRequiredService<IUseSend>());
        services.AddScoped<IContactBookService>(sp => (IContactBookService)sp.GetRequiredService<IUseSend>());
        services.AddScoped<ICampaignService>(sp => (ICampaignService)sp.GetRequiredService<IUseSend>());
        services.AddScoped<IAnalyticsService>(sp => (IAnalyticsService)sp.GetRequiredService<IUseSend>());

        return services.AddHttpClient<IUseSend, UseSendClient>();
    }
}