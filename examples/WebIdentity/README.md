# WebIdentity Example

Demonstrates ASP.NET Core Identity + useSend using `UseSend.Identity`.

## What it shows

- Registering a user and sending a **confirmation email** via `IEmailSender<TUser>.SendConfirmationLinkAsync`
- Confirming an email address
- Sending a **password reset link** via `SendPasswordResetLinkAsync`
- Resetting a password
- How to **override email templates** by subclassing `UseSendEmailSender<TUser>`

Uses an in-memory EF Core database — no setup required.

## Running

```bash
# Set your API token (or edit appsettings.Development.json)
export USESEND_API_KEY=us_your_api_token

dotnet run
```

## Endpoints

| Method | Path | Description |
|--------|------|-------------|
| `POST` | `/register` | Creates a user and sends a confirmation email |
| `GET` | `/confirm-email?userId=&token=` | Confirms the email address |
| `POST` | `/forgot-password` | Sends a password reset link |
| `POST` | `/reset-password` | Resets the password using a token |

## Example requests

```http
POST /register
Content-Type: application/json

{ "email": "alice@example.com", "password": "P@ssword1!" }
```

```http
POST /forgot-password
Content-Type: application/json

{ "email": "alice@example.com" }
```

## Customising email templates

Subclass `UseSendEmailSender<TUser>` and override any of the three virtual methods:

```csharp
public class MyEmailSender(IEmailService emails, EmailSenderOptions options)
    : UseSendEmailSender<AppUser>(emails, options)
{
    public override Task SendConfirmationLinkAsync(AppUser user, string email, string link) =>
        SendEmailAsync(email, "Please verify your email",
            $"<h1>Hi {user.UserName}!</h1><p><a href='{link}'>Confirm my email</a></p>");
}

// In Program.cs — replace AddUseSendIdentityEmailSender<AppUser> with:
builder.Services.AddSingleton(new EmailSenderOptions("noreply@example.com", "My App"));
builder.Services.AddTransient<IEmailSender<AppUser>, MyEmailSender>();
builder.Services.AddTransient<IEmailSender, MyEmailSender>();
```
