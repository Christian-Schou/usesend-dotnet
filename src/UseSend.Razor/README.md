# UseSend.Razor

[![NuGet](https://img.shields.io/nuget/v/UseSend.Razor.svg)](https://www.nuget.org/packages/UseSend.Razor/)

Razor (.cshtml) template rendering for the [useSend](https://usesend.com) .NET SDK — compile and render strongly-typed Razor templates as email HTML bodies using [RazorLight](https://github.com/toddams/RazorLight).

## Installation

```bash
dotnet add package UseSend
dotnet add package UseSend.Razor
```

## Quick start

### 1. Create a template

`Templates/Emails/Welcome.cshtml`:

```html
@model MyApp.Emails.WelcomeModel
<!DOCTYPE html>
<html>
<body>
  <h1>Welcome, @Model.Name!</h1>
  <p>Click <a href="@Model.ConfirmUrl">here</a> to confirm your email address.</p>
</body>
</html>
```

### 2. Register services

```csharp
builder.Services.AddUseSend("us_yourtoken");
builder.Services.AddUseSendRazor();                          // uses ./Templates by default
// or:
builder.Services.AddUseSendRazor("/absolute/path/to/templates");
// or with full options:
builder.Services.AddUseSendRazor(opts =>
    opts.TemplateRootPath = Path.Combine(AppContext.BaseDirectory, "EmailTemplates"));
```

### 3. Render and send

```csharp
public class WelcomeEmailService(IUseSend useSend, IEmailTemplateRenderer renderer)
{
    public async Task SendWelcomeAsync(string email, string name, string confirmUrl)
    {
        var html = await renderer.RenderAsync("Emails/Welcome", new WelcomeModel
        {
            Name = name,
            ConfirmUrl = confirmUrl
        });

        await useSend.EmailSendAsync(new EmailMessage
        {
            From    = "hello@myapp.com",
            To      = email,
            Subject = $"Welcome, {name}!",
            HtmlBody = html
        });
    }
}
```

## Template keys

The `templateKey` argument is the path **relative to `TemplateRootPath`**, without the `.cshtml` extension:

| Template file | Key |
|---------------|-----|
| `Templates/Emails/Welcome.cshtml` | `"Emails/Welcome"` |
| `Templates/Reset.cshtml` | `"Reset"` |

## Requirements

- .NET 8 or .NET 10
- `UseSend` ≥ 1.4.0
- `RazorLight` ≥ 2.3.1
