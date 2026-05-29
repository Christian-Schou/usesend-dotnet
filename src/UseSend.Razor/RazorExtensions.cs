using Microsoft.Extensions.DependencyInjection;

namespace UseSend.Razor;

/// <summary>
///     Extension methods for registering Razor email templating with the DI container.
/// </summary>
public static class RazorExtensions
{
    /// <summary>
    ///     Registers <see cref="IEmailTemplateRenderer" /> using RazorLight with file-system templates
    ///     loaded from <paramref name="templateRootPath" />.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="templateRootPath">
    ///     Absolute path to the directory containing .cshtml email templates.
    ///     If <see langword="null" />, defaults to a <c>Templates</c> subfolder in the current working directory.
    /// </param>
    public static IServiceCollection AddUseSendRazor(
        this IServiceCollection services,
        string? templateRootPath = null)
    {
        var options = new EmailTemplateOptions();
        if (templateRootPath is not null)
            options.TemplateRootPath = templateRootPath;

        services.AddSingleton(options);
        services.AddSingleton<IEmailTemplateRenderer, RazorLightEmailTemplateRenderer>();

        return services;
    }

    /// <summary>
    ///     Registers <see cref="IEmailTemplateRenderer" /> using RazorLight, configuring options via a delegate.
    /// </summary>
    public static IServiceCollection AddUseSendRazor(
        this IServiceCollection services,
        Action<EmailTemplateOptions> configure)
    {
        var options = new EmailTemplateOptions();
        configure(options);

        services.AddSingleton(options);
        services.AddSingleton<IEmailTemplateRenderer, RazorLightEmailTemplateRenderer>();

        return services;
    }
}
