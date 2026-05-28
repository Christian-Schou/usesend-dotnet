# UseSend.FluentEmail

[![NuGet](https://img.shields.io/nuget/v/UseSend.FluentEmail.svg)](https://www.nuget.org/packages/UseSend.FluentEmail)

[FluentEmail](https://github.com/lukencode/FluentEmail) sender backed by the [useSend](https://usesend.com) API.

## Installation

```bash
dotnet add package UseSend
dotnet add package UseSend.FluentEmail
```

## Usage

### With Dependency Injection

```csharp
// Program.cs
builder.Services.AddUseSend("us_your_api_token");

builder.Services
    .AddFluentEmail("noreply@yourdomain.com")
    .AddUseSendSender();
```

```csharp
// In your service
public class WelcomeSender(IFluentEmail email)
{
    public async Task SendAsync(string to)
    {
        await email
            .To(to)
            .Subject("Welcome!")
            .Body("<h1>Hello!</h1>", isHtml: true)
            .SendAsync();
    }
}
```

### Without Dependency Injection

```csharp
var client  = UseSendClient.Create("us_your_api_token");
var sender  = new UseSendSender(client.Emails);

Email.DefaultSender = sender;

await Email.From("noreply@yourdomain.com")
    .To("user@example.com")
    .Subject("Hello")
    .Body("Hello from useSend!")
    .SendAsync();
```

## Supported features

| FluentEmail feature | Supported |
|---------------------|-----------|
| To / CC / BCC       | ✅         |
| ReplyTo             | ✅         |
| HTML body           | ✅         |
| Plain-text body     | ✅         |
| Plain-text fallback | ✅         |
| Attachments         | ✅         |
| Named addresses     | ✅         |
