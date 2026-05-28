using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UseSend.Identity;

/// <summary>
///     Extension methods for registering the useSend Identity email sender.
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    ///     Registers <see cref="UseSendEmailSender" /> as the ASP.NET Core Identity <see cref="IEmailSender" />.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="fromAddress">The email address to send from.</param>
    /// <param name="fromName">Optional display name, e.g. "My App".</param>
    public static IServiceCollection AddUseSendIdentityEmailSender(
        this IServiceCollection services,
        string fromAddress,
        string? fromName = null)
    {
        services.AddSingleton(new EmailSenderOptions(fromAddress, fromName));
        services.AddTransient<IEmailSender, UseSendEmailSender>();
        return services;
    }

    /// <summary>
    ///     Registers <see cref="UseSendEmailSender{TUser}" /> as both <see cref="IEmailSender" /> and
    ///     <see cref="IEmailSender{TUser}" /> for use with typed ASP.NET Core Identity.
    /// </summary>
    /// <typeparam name="TUser">The Identity user type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="fromAddress">The email address to send from.</param>
    /// <param name="fromName">Optional display name, e.g. "My App".</param>
    public static IServiceCollection AddUseSendIdentityEmailSender<TUser>(
        this IServiceCollection services,
        string fromAddress,
        string? fromName = null) where TUser : class
    {
        services.AddSingleton(new EmailSenderOptions(fromAddress, fromName));
        services.AddTransient<IEmailSender<TUser>, UseSendEmailSender<TUser>>();
        services.AddTransient<IEmailSender, UseSendEmailSender<TUser>>();
        return services;
    }
}
