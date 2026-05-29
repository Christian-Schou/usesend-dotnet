# UseSend.OpenTelemetry

[![NuGet](https://img.shields.io/nuget/v/UseSend.OpenTelemetry.svg)](https://www.nuget.org/packages/UseSend.OpenTelemetry/)

OpenTelemetry instrumentation for the [useSend](https://usesend.com) .NET SDK — adds distributed tracing (spans) and metrics (request count + duration histogram) to every useSend API call.

## Installation

```bash
dotnet add package UseSend.OpenTelemetry
```

## Usage

```csharp
using UseSend.OpenTelemetry;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddUseSendInstrumentation()   // adds "UseSend" ActivitySource
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddUseSendInstrumentation()   // adds "UseSend" Meter
        .AddConsoleExporter());
```

## What is instrumented?

Every HTTP request made by `IUseSend` is wrapped in an OpenTelemetry `Activity` with:

| Tag | Example |
|-----|---------|
| `http.method` | `POST` |
| `http.url` | `https://app.usesend.com/api/v1/emails` |
| `http.status_code` | `200` |
| Activity status | `Error` on non-2xx |

### Metrics

| Metric | Type | Description |
|--------|------|-------------|
| `usesend.client.requests` | Counter | Total number of requests |
| `usesend.client.request_duration` | Histogram (ms) | Round-trip duration per request |

Both metrics include `http.method` and `http.status_code` dimensions.

## Requirements

- .NET 8 or .NET 10
- `UseSend` ≥ 1.4.0
- `OpenTelemetry` ≥ 1.9
