# UseSend.Fluid

[![NuGet](https://img.shields.io/nuget/v/UseSend.Fluid.svg)](https://www.nuget.org/packages/UseSend.Fluid/)

[Liquid](https://shopify.github.io/liquid/) template rendering for the [useSend](https://usesend.com) .NET SDK — compile and render strongly-typed `.liquid` email templates using [Fluid](https://github.com/sebastienros/fluid).

Liquid templates are simpler than Razor, safe to hand off to designers or marketers, and support all standard Liquid tags (`{% if %}`, `{% for %}`, filters, etc.).

## Installation

```bash
dotnet add package UseSend
dotnet add package UseSend.Fluid
```

## Quick start

### 1. Create a template

`Templates/Emails/Welcome.liquid`:

```liquid
<!DOCTYPE html>
<html>
<body>
  <h1>Welcome, {{ Name }}!</h1>
  <p>Click <a href="{{ ConfirmUrl }}">here</a> to confirm your email address.</p>
  {% if HasPromo %}
  <p>Use code <strong>{{ PromoCode }}</strong> for 20% off.</p>
  {% endif %}
</body>
</html>
```

### 2. Register services

```csharp
builder.Services.AddUseSend("us_yourtoken");
builder.Services.AddUseSendFluid();                          // uses ./Templates by default
// or:
builder.Services.AddUseSendFluid("/absolute/path/to/templates");
// or with full options:
builder.Services.AddUseSendFluid(opts =>
    opts.TemplateRootPath = Path.Combine(AppContext.BaseDirectory, "EmailTemplates"));
```

### 3. Render and send

```csharp
public class WelcomeEmailService(IUseSend useSend, IEmailTemplateRenderer renderer)
{
    public async Task SendWelcomeAsync(string email, string name, string confirmUrl)
    {
        var html = await renderer.RenderAsync("Emails/Welcome", new
        {
            Name       = name,
            ConfirmUrl = confirmUrl,
            HasPromo   = true,
            PromoCode  = "WELCOME20"
        });

        await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "hello@myapp.com",
            To      = email,
            Subject = $"Welcome, {name}!",
            Html    = html
        });
    }
}
```

## Template keys

The `templateKey` argument is the path **relative to `TemplateRootPath`**, without the `.liquid` extension:

| Template file | Key |
|---------------|-----|
| `Templates/Emails/Welcome.liquid` | `"Emails/Welcome"` |
| `Templates/Reset.liquid` | `"Reset"` |

## Template caching

Parsed templates are cached in-memory after the first render. To reload templates (e.g. during development), restart the application or inject a new `FluidEmailTemplateRenderer`.

## Swapping between Fluid and Razor

Both `UseSend.Fluid` and `UseSend.Razor` implement the same `IEmailTemplateRenderer` interface (defined in `UseSend` core). To switch renderer, change only the registration:

```csharp
// Razor
builder.Services.AddUseSendRazor();

// Liquid — swap this one line, no other code changes needed
builder.Services.AddUseSendFluid();
```

## Requirements

- .NET 8 or .NET 10
- `UseSend` ≥ 1.4.0
- `Fluid.Core` ≥ 2.11
