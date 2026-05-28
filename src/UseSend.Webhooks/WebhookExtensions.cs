using Microsoft.Extensions.DependencyInjection;

namespace UseSend.Webhooks;

/// <summary>
///     Extension methods for registering useSend webhook services.
/// </summary>
public static class WebhookExtensions
{
    /// <summary>
    ///     Registers <see cref="UseSendWebhooks" /> as a singleton in the service container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="signingSecret">The webhook signing secret from your useSend dashboard.</param>
    public static IServiceCollection AddUseSendWebhooks(this IServiceCollection services, string signingSecret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(signingSecret);

        services.AddSingleton(new UseSendWebhooks(signingSecret));
        return services;
    }
}