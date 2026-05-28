// ConsoleNoDi — useSend SDK without dependency injection
// Usage: set USESEND_API_KEY or replace UseSendClient.Create() with UseSendClient.Create("us_...")

using UseSend;

var client = UseSendClient.Create(); // reads USESEND_API_KEY env var

var response = await client.Emails.SendAsync(new EmailMessage
{
    From = "noreply@yourdomain.com",
    To = "recipient@example.com",
    Subject = "Hello from useSend!",
    Html = "<h1>Hello!</h1><p>This email was sent using the useSend .NET SDK.</p>",
    Text = "Hello! This email was sent using the useSend .NET SDK."
});

Console.WriteLine(response.Success
    ? $"Email sent successfully. ID: {response.Content}"
    : $"Failed to send email: {response.Exception?.Message}");