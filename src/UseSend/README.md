# useSend .NET SDK

[![NuGet](https://img.shields.io/nuget/v/UseSend.svg)](https://www.nuget.org/packages/UseSend)

.NET client for the [useSend](https://usesend.com) email API. useSend is an open-source alternative to Resend, SendGrid,
Mailgun and Postmark.

## Installation

```bash
dotnet add package UseSend
```

## Quick Start

### With Dependency Injection (ASP.NET Core)

```csharp
// Program.cs
builder.Services.AddUseSend("us_your_api_token");

// In your service/controller — inject IUseSend or a specific service interface
public class WelcomeSender(IUseSend useSend)
{
    public async Task SendWelcomeAsync(string to)
    {
        var response = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "noreply@yourdomain.com",
            To      = to,
            Subject = "Welcome!",
            Html    = "<h1>Welcome aboard!</h1>",
        });

        if (response.Success)
            Console.WriteLine($"Email sent: {response.Content}");
    }
}

// Or inject only the service interface you need
public class DomainChecker(IEmailService emails, IDomainService domains) { ... }
```

### Self-Hosted useSend

```csharp
builder.Services.AddUseSend(options =>
{
    options.ApiToken = "us_your_api_token";
    options.ApiUrl   = "https://send.mycompany.com/api/"; // your self-hosted instance
});
```

### Without Dependency Injection

```csharp
var client = UseSendClient.Create("us_your_api_token");

var response = await client.Emails.SendAsync(new EmailMessage
{
    From    = "noreply@yourdomain.com",
    To      = "user@example.com",
    Subject = "Hello",
    Text    = "Hello from useSend!",
});
```

### Environment Variable

If you prefer not to hardcode your token, set the `USESEND_API_KEY` environment variable and create the client with no
arguments:

```csharp
var client = UseSendClient.Create(); // reads USESEND_API_KEY
```

## Features

| Resource          | Operations                                                                            |
|-------------------|---------------------------------------------------------------------------------------|
| **Emails**        | Send, Batch send (both with idempotency), Get, List, Cancel schedule, Update schedule |
| **Domains**       | List, Create, Get, Delete, Verify                                                     |
| **Contacts**      | Create, Get, List, Update, Upsert, Delete, Bulk create, Bulk delete                   |
| **Contact Books** | List, Create, Get, Update, Delete                                                     |
| **Campaigns**     | Create, Get, List, Delete, Schedule, Pause, Resume                                    |
| **Analytics**     | Email time series, Reputation metrics                                                 |

## Examples

- [ConsoleNoDi](examples/ConsoleNoDi) — Standalone usage without dependency injection
- [WebMinimalApi](examples/WebMinimalApi) — ASP.NET Core minimal API with DI
- [ConsoleSelfHosted](examples/ConsoleSelfHosted) — Custom host URL for self-hosted useSend
- [WebIdentity](examples/WebIdentity) — ASP.NET Core Identity with email confirmation and password reset

## Authentication

All API calls require a Bearer token. Create one in
your [useSend Developer Settings](https://app.usesend.com/dev-settings/api-keys).

## Error Handling

By default, failed API calls throw a `UseSendException`. To receive errors as return values instead:

```csharp
builder.Services.AddUseSend(options =>
{
    options.ApiToken        = "us_your_api_token";
    options.ThrowExceptions = false;
});

var response = await useSend.Emails.SendAsync(email);

if (!response.Success)
    Console.WriteLine($"Error {response.Exception!.StatusCode}: {response.Exception.ApiError}");
```

## API Reference

Full API documentation: [docs.usesend.com/api-reference](https://docs.usesend.com/api-reference/introduction)
