using Microsoft.Extensions.DependencyInjection;

namespace UseSend.Fluid;

/// <summary>
///     Extension methods for registering Fluid (Liquid) email templating with the DI container.
/// </summary>
public static class FluidExtensions
{
    /// <summary>
    ///     Registers <see cref="IEmailTemplateRenderer" /> using the Fluid Liquid engine with file-system
    ///     templates loaded from <paramref name="templateRootPath" />.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="templateRootPath">
    ///     Absolute path to the directory containing <c>.liquid</c> email templates.
    ///     If <see langword="null" />, defaults to a <c>Templates</c> subfolder in the current working directory.
    /// </param>
    public static IServiceCollection AddUseSendFluid(
        this IServiceCollection services,
        string? templateRootPath = null)
    {
        var options = new FluidEmailTemplateOptions();
        if (templateRootPath is not null)
            options.TemplateRootPath = templateRootPath;

        services.AddSingleton(options);
        services.AddSingleton<IEmailTemplateRenderer, FluidEmailTemplateRenderer>();

        return services;
    }

    /// <summary>
    ///     Registers <see cref="IEmailTemplateRenderer" /> using the Fluid Liquid engine, configuring
    ///     options via a delegate.
    /// </summary>
    public static IServiceCollection AddUseSendFluid(
        this IServiceCollection services,
        Action<FluidEmailTemplateOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var options = new FluidEmailTemplateOptions();
        configure(options);

        services.AddSingleton(options);
        services.AddSingleton<IEmailTemplateRenderer, FluidEmailTemplateRenderer>();

        return services;
    }
}
