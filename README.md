# useSend .NET SDK

Unofficial .NET SDK for [useSend](https://usesend.com) — an open-source alternative to Resend, SendGrid, Mailgun, and Postmark.

[![CI](https://github.com/Christian-Schou/usesend-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/your-org/usesend-dotnet/actions/workflows/ci.yml)

## Packages

| Package | NuGet | Description |
|---------|-------|-------------|
| `UseSend` | [![NuGet](https://img.shields.io/nuget/v/UseSend.svg)](https://www.nuget.org/packages/UseSend) | Core SDK — send emails, manage domains, contacts, campaigns, and analytics |
| `UseSend.FluentEmail` | [![NuGet](https://img.shields.io/nuget/v/UseSend.FluentEmail.svg)](https://www.nuget.org/packages/UseSend.FluentEmail) | [FluentEmail](https://github.com/lukencode/FluentEmail) sender backed by useSend |
| `UseSend.Webhooks` | [![NuGet](https://img.shields.io/nuget/v/UseSend.Webhooks.svg)](https://www.nuget.org/packages/UseSend.Webhooks) | Webhook signature verification and typed event parsing |

---

## UseSend — Core SDK

```bash
dotnet add package UseSend
```

```csharp
// Program.cs
builder.Services.AddUseSend("us_your_api_token");

// Inject IUseSend or individual service interfaces (IEmailService, IDomainService, …)
public class WelcomeSender(IUseSend useSend)
{
    public async Task SendAsync(string to)
    {
        var response = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "noreply@yourdomain.com",
            To      = to,
            Subject = "Welcome!",
            Html    = "<h1>Welcome aboard!</h1>",
        });
    }
}
```

**Self-hosted useSend** — point the SDK at your own instance:

```csharp
builder.Services.AddUseSend(options =>
{
    options.ApiToken = "us_your_api_token";
    options.ApiUrl   = "https://send.mycompany.com/api/";
});
```

| Resource | Operations |
|----------|-----------|
| **Emails** | Send, Batch send (with idempotency), Get, List, Cancel / Update schedule |
| **Domains** | List, Create, Get, Delete, Verify |
| **Contacts** | Create, Get, List, Update, Upsert, Delete, Bulk create / delete |
| **Contact Books** | List, Create, Get, Update, Delete |
| **Campaigns** | Create, Get, List, Delete, Schedule, Pause, Resume |
| **Analytics** | Email time series, Reputation metrics |

→ [Full documentation](src/UseSend/README.md)

---

## UseSend.FluentEmail

Drop-in [FluentEmail](https://github.com/lukencode/FluentEmail) sender — use the FluentEmail API you already know, delivered by useSend.

```bash
dotnet add package UseSend
dotnet add package UseSend.FluentEmail
```

```csharp
builder.Services.AddUseSend("us_your_api_token");
builder.Services
    .AddFluentEmail("noreply@yourdomain.com")
    .AddUseSendSender();
```

→ [Full documentation](src/UseSend.FluentEmail/README.md)

---

## UseSend.Webhooks

Verify webhook signatures and parse typed event payloads from useSend.

```bash
dotnet add package UseSend.Webhooks
```

```csharp
builder.Services.AddUseSendWebhooks(builder.Configuration["UseSend:WebhookSecret"]!);

app.MapPost("/webhooks/usesend", async (HttpRequest req, UseSendWebhooks webhooks) =>
{
    var rawBody = await new StreamReader(req.Body).ReadToEndAsync();
    var headers = req.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

    var evt = webhooks.ConstructEvent(rawBody, headers); // throws WebhookException on bad/stale signature

    if (evt.Type == WebhookEventType.EmailDelivered)
        Console.WriteLine($"Delivered: {evt.GetData<EmailEventData>()?.Id}");

    return Results.Ok();
});
```

Signatures use `HMAC-SHA256` with constant-time comparison and 5-minute replay protection.

| Group | Events |
|-------|--------|
| Email | `queued`, `sent`, `delivered`, `delivery_delayed`, `bounced`, `rejected`, `rendering_failure`, `complained`, `failed`, `cancelled`, `suppressed`, `opened`, `clicked` |
| Contact | `created`, `updated`, `deleted` |
| Domain | `created`, `verified`, `updated`, `deleted` |

→ [Full documentation](src/UseSend.Webhooks/README.md)

---

## Requirements

- .NET 10+
- A [useSend](https://usesend.com) account or self-hosted instance

## Contributing

PRs and issues welcome. Run tests with:

```bash
dotnet test
```

## License

MIT
