using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace UseSend.OpenTelemetry;

/// <summary>
///     OpenTelemetry extension methods for adding useSend instrumentation.
/// </summary>
public static class UseSendInstrumentationExtensions
{
    /// <summary>
    ///     Adds useSend distributed tracing (ActivitySource) to the <see cref="TracerProviderBuilder" />.
    /// </summary>
    public static TracerProviderBuilder AddUseSendInstrumentation(this TracerProviderBuilder builder)
        => builder.AddSource(UseSendTelemetry.SourceName);

    /// <summary>
    ///     Adds useSend metrics (request count + duration) to the <see cref="MeterProviderBuilder" />.
    /// </summary>
    public static MeterProviderBuilder AddUseSendInstrumentation(this MeterProviderBuilder builder)
        => builder.AddMeter(UseSendTelemetry.MeterName);
}
