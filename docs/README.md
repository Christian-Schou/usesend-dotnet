# UseSend .NET SDK — Documentation

## Overview

**UseSend** is a .NET 10 SDK for the [useSend](https://usesend.com) email API — an open-source, self-hostable alternative to Resend.

The SDK provides:
- Typed methods for every useSend API endpoint
- Dependency injection support via `IServiceCollection.AddUseSend()`
- Standalone (no-DI) usage via `UseSendClient.Create()`
- Self-hosted instance support via `UseSendClientOptions.ApiUrl`
- Configurable error handling (`ThrowExceptions` flag)
- Full async/await with `CancellationToken` support

---

## API Coverage

| Resource         | Operations |
|------------------|------------|
| **Emails**       | Send, Get, List, Batch send, Cancel schedule, Update schedule (all with optional idempotency key) |
| **Domains**      | List, Create, Get, Delete, Verify |
| **Contacts**     | Create, Get, List, Update, Upsert, Delete, Bulk create, Bulk delete |
| **Contact Books**| List, Create, Get, Update, Delete |
| **Campaigns**    | Create, Get, List, Delete, Schedule, Pause, Resume |
| **Analytics**    | Email time series, Reputation metrics |

---

## Quick Start

### Install

```bash
dotnet add package UseSend
```

### With Dependency Injection (ASP.NET Core)

```csharp
// Program.cs
builder.Services.AddUseSend("us_yourtoken");

// Or for a self-hosted instance:
builder.Services.AddUseSend(opt =>
{
    opt.ApiToken = "us_yourtoken";
    opt.ApiUrl   = "https://send.yourcompany.com/api/";
});
```

```csharp
// EmailController.cs
public class EmailController(IUseSend useSend)
{
    public async Task<IActionResult> SendWelcome()
    {
        var result = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "you@example.com",
            To      = new List<string> { "user@example.com" },
            Subject = "Welcome!",
            Html    = "<p>Hello!</p>",
        });

        return result.Success ? Ok() : StatusCode(500);
    }
}

// You can also inject individual service interfaces:
public class DomainChecker(IDomainService domains) { ... }
```

### Without Dependency Injection

```csharp
var client = UseSendClient.Create("us_yourtoken");

// Self-hosted
var client = UseSendClient.Create(new UseSendClientOptions
{
    ApiToken = "us_yourtoken",
    ApiUrl   = "https://send.yourcompany.com/api/",
});
```

---

## Configuration

| Property          | Default                              | Description |
|-------------------|--------------------------------------|-------------|
| `ApiToken`        | `USESEND_API_KEY` env var            | Your useSend API key |
| `ApiUrl`          | `https://app.usesend.com/api/`       | Base URL (override for self-hosted) |
| `ThrowExceptions` | `true`                               | Throw `UseSendException` on API errors, or return in `UseSendResponse.Exception` |

---

## Error Handling

```csharp
// Throw mode (default) — exceptions on error
try
{
    var result = await client.Emails.SendAsync(message);
}
catch (UseSendException ex)
{
    Console.WriteLine($"Error {ex.StatusCode}: {ex.Message}");
}

// Non-throw mode — check Success property
var client = UseSendClient.Create(new UseSendClientOptions
{
    ApiToken        = "us_yourtoken",
    ThrowExceptions = false,
});

var result = await client.Emails.SendAsync(message);
if (!result.Success)
    Console.WriteLine(result.Exception?.Message);
```

---

## Examples

See the [examples/](../examples/) directory:

| Example              | Description |
|----------------------|-------------|
| `ConsoleNoDi`        | Standalone client without DI — minimal setup |
| `WebMinimalApi`      | ASP.NET Core Minimal API with DI registration |
| `ConsoleSelfHosted`  | Custom `ApiUrl` for a self-hosted useSend instance |

---

## Links

- [useSend Website](https://usesend.com)
- [API Reference](https://docs.usesend.com/api-reference/introduction)
- [NuGet Package](https://www.nuget.org/packages/UseSend)
- [GitHub](https://github.com/your-org/usesend-dotnet)
