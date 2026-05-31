# WebFluid example

Minimal API that renders Liquid (`.liquid`) email templates using **UseSend.Fluid** and sends them via the **UseSend** SDK.

## What it shows

- Registering `IEmailTemplateRenderer` via `AddUseSendFluid()`
- Two Liquid templates with model interpolation and an `{% if %}` conditional:
  - `Templates/Emails/Welcome.liquid` — welcome + optional promo code
  - `Templates/Emails/PasswordReset.liquid` — password reset link with expiry
- Injecting `IEmailTemplateRenderer` directly into Minimal API endpoint handlers

## Configuration

Set your API token in `appsettings.Development.json` or via an environment variable:

```bash
export USESEND_API_KEY=us_your_token_here
```

| Key | Default | Description |
|-----|---------|-------------|
| `UseSend:ApiToken` | — | **Required.** Your useSend API token |
| `UseSend:ApiUrl` | useSend cloud | Override for self-hosted instances |
| `UseSend:FromAddress` | `noreply@example.com` | Sender address |
| `UseSend:FromName` | `My App` | Sender display name |
| `App:Name` | `My App` | App name injected into templates |

## Run

```bash
dotnet run --project examples/WebFluid
```

## Endpoints

### `POST /send-welcome`

```json
{
  "email": "alice@example.com",
  "name": "Alice",
  "confirmUrl": "https://myapp.com/confirm?token=abc123",
  "promoCode": "WELCOME20"
}
```

Omit `promoCode` (or pass `null`) to skip the promotional block in the template.

### `POST /send-password-reset`

```json
{
  "email": "alice@example.com",
  "name": "Alice",
  "resetUrl": "https://myapp.com/reset?token=xyz789"
}
```

## Swapping to Razor

Both `UseSend.Fluid` and `UseSend.Razor` implement `IEmailTemplateRenderer`. To switch to Razor, change **one line** in `Program.cs`:

```csharp
// Liquid (current)
builder.Services.AddUseSendFluid(opts => ...);

// Razor — swap this line, nothing else changes
builder.Services.AddUseSendRazor(opts => ...);
```
