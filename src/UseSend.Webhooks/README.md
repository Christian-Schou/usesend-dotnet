# UseSend.Webhooks

[![NuGet](https://img.shields.io/nuget/v/UseSend.Webhooks.svg)](https://www.nuget.org/packages/UseSend.Webhooks)

Webhook signature verification and event parsing for [useSend](https://usesend.com).

## Installation

```bash
dotnet add package UseSend.Webhooks
```

## Usage

### With Dependency Injection (ASP.NET Core)

```csharp
// Program.cs
builder.Services.AddUseSendWebhooks(builder.Configuration["UseSend:WebhookSecret"]!);

app.MapPost("/webhooks/usesend", async (HttpRequest req, UseSendWebhooks webhooks) =>
{
    var rawBody = await new StreamReader(req.Body).ReadToEndAsync();

    var headers = req.Headers
        .ToDictionary(h => h.Key, h => h.Value.ToString());

    WebhookEvent evt;
    try
    {
        evt = webhooks.ConstructEvent(rawBody, headers);
    }
    catch (WebhookException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    switch (evt.Type)
    {
        case WebhookEventType.EmailDelivered:
            var data = evt.GetData<EmailEventData>();
            Console.WriteLine($"Delivered to: {string.Join(", ", data!.To)}");
            break;

        case WebhookEventType.EmailBounced:
            Console.WriteLine($"Bounced: {evt.GetData<EmailEventData>()?.Id}");
            break;

        case WebhookEventType.ContactCreated:
            Console.WriteLine($"New contact: {evt.GetData<ContactEventData>()?.Email}");
            break;
    }

    return Results.Ok();
});
```

### Signature verification only

```csharp
var isValid = webhooks.Verify(rawBody, headers);
```

## Security

- Signatures use `HMAC-SHA256(secret, "{timestamp}.{rawBody}")` compared with constant-time equality
- Requests older than **5 minutes** are automatically rejected (replay attack prevention)
- Signature format: `v1=<hex>` in the `X-UseSend-Signature` header

## Event types

See `WebhookEventType` for all 20 event type constants across emails, contacts, and domains.

| Group   | Events                                                                                                                                                                |
|---------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Email   | `queued`, `sent`, `delivered`, `delivery_delayed`, `bounced`, `rejected`, `rendering_failure`, `complained`, `failed`, `cancelled`, `suppressed`, `opened`, `clicked` |
| Contact | `created`, `updated`, `deleted`                                                                                                                                       |
| Domain  | `created`, `verified`, `updated`, `deleted`                                                                                                                           |
