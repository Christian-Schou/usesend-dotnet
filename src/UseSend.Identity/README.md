# UseSend.Identity

[![NuGet](https://img.shields.io/nuget/v/UseSend.Identity.svg)](https://www.nuget.org/packages/UseSend.Identity)

ASP.NET Core Identity `IEmailSender` / `IEmailSender<TUser>` implementation backed by the [useSend](https://usesend.com) API.

## Installation

```bash
dotnet add package UseSend
dotnet add package UseSend.Identity
```

## Usage

### Non-generic `IEmailSender`

Suitable for custom Identity flows or any code that injects `IEmailSender`.

```csharp
// Program.cs
builder.Services.AddUseSend("us_your_api_token");
builder.Services.AddUseSendIdentityEmailSender("noreply@yourdomain.com", fromName: "My App");
```

```csharp
// In your service
public class AccountService(IEmailSender emailSender)
{
    public Task NotifyAsync(string email) =>
        emailSender.SendEmailAsync(email, "Hello", "<p>Hello from useSend!</p>");
}
```

### Generic `IEmailSender<TUser>` (scaffolded Identity pages)

Use this when your project uses scaffolded ASP.NET Core Identity pages (confirmation emails, password resets, etc.).

```csharp
// Program.cs
builder.Services.AddUseSend("us_your_api_token");
builder.Services.AddUseSendIdentityEmailSender<ApplicationUser>(
    fromAddress: "noreply@yourdomain.com",
    fromName: "My App"
);
```

This registers both `IEmailSender` and `IEmailSender<ApplicationUser>` so all Identity scaffolding works out of the box.

### Customising email templates

Subclass `UseSendEmailSender<TUser>` and override any of the three virtual methods:

```csharp
public class MyEmailSender(IEmailService emails, EmailSenderOptions options)
    : UseSendEmailSender<ApplicationUser>(emails, options)
{
    public override Task SendConfirmationLinkAsync(ApplicationUser user, string email, string link) =>
        SendEmailAsync(email, "Verify your email",
            $"<h1>Hi {user.UserName}!</h1><p><a href='{link}'>Confirm email</a></p>");
}

// Program.cs
builder.Services.AddSingleton(new EmailSenderOptions("noreply@yourdomain.com", "My App"));
builder.Services.AddTransient<IEmailSender<ApplicationUser>, MyEmailSender>();
builder.Services.AddTransient<IEmailSender, MyEmailSender>();
```

## Default email templates

| Method | Subject | Body |
|--------|---------|------|
| `SendConfirmationLinkAsync` | Confirm your email address | Link to confirm account |
| `SendPasswordResetLinkAsync` | Reset your password | Link to reset password |
| `SendPasswordResetCodeAsync` | Reset your password | Bold reset code |
