namespace UseSend.OpenTelemetry;

/// <summary>
///     Constants for useSend telemetry source and meter names.
/// </summary>
public static class UseSendTelemetry
{
    /// <summary>The ActivitySource name used for distributed tracing spans.</summary>
    public const string SourceName = "UseSend";

    /// <summary>The Meter name used for metrics (request count, duration).</summary>
    public const string MeterName = "UseSend";
}
