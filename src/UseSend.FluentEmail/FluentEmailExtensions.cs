using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace UseSend.FluentEmail;

/// <summary>
///     Extension methods for registering the useSend FluentEmail sender.
/// </summary>
public static class FluentEmailExtensions
{
    /// <summary>
    ///     Adds the useSend sender to FluentEmail.
    ///     Call this after <c>AddFluentEmail()</c>.
    /// </summary>
    /// <param name="builder">The FluentEmail builder.</param>
    public static FluentEmailServicesBuilder AddUseSendSender(this FluentEmailServicesBuilder builder)
    {
        builder.Services.AddScoped<ISender, UseSendSender>();
        return builder;
    }
}