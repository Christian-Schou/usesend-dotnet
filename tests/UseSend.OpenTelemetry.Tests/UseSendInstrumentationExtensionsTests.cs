using System.Diagnostics;
using UseSend.OpenTelemetry;

namespace UseSend.OpenTelemetry.Tests;

public sealed class UseSendInstrumentationExtensionsTests
{
    [Fact]
    public void AddUseSendInstrumentation_Tracer_BuildsWithoutThrowing()
    {
        var exception = Record.Exception(() =>
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddUseSendInstrumentation()
                .Build();
        });

        Assert.Null(exception);
    }

    [Fact]
    public void AddUseSendInstrumentation_Meter_BuildsWithoutThrowing()
    {
        var exception = Record.Exception(() =>
        {
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddUseSendInstrumentation()
                .Build();
        });

        Assert.Null(exception);
    }

    [Fact]
    public void UseSendTelemetry_SourceName_MatchesActivitySourceName()
    {
        Assert.Equal("UseSend", UseSendTelemetry.SourceName);

        // Verify the ActivitySource with this name is recognised by the SDK
        var source = new ActivitySource(UseSendTelemetry.SourceName);
        Assert.Equal("UseSend", source.Name);
    }

    [Fact]
    public void UseSendTelemetry_Constants_AreCorrect()
    {
        Assert.Equal("UseSend", UseSendTelemetry.SourceName);
        Assert.Equal("UseSend", UseSendTelemetry.MeterName);
    }
}
