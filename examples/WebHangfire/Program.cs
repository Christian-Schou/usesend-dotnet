using Hangfire;
using Hangfire.InMemory;
using UseSend;

var builder = WebApplication.CreateBuilder(args);

// --- useSend ---
builder.Services.AddUseSend(builder.Configuration["UseSend:ApiToken"]
    ?? throw new InvalidOperationException("UseSend:ApiToken not configured."));

// --- Hangfire (in-memory storage for demo; swap for SqlServer/Redis in production) ---
builder.Services.AddHangfire(cfg => cfg.UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

// Trigger a fire-and-forget email job via HTTP for demo purposes
app.MapPost("/send-welcome", (string to, IBackgroundJobClient jobs) =>
{
    var jobId = jobs.Enqueue<EmailJob>(j => j.SendWelcomeAsync(to, CancellationToken.None));
    return Results.Ok(new { jobId });
});

// Hangfire dashboard (dev only — add auth middleware in production)
app.MapHangfireDashboard("/hangfire");

app.Run();

/// <summary>Hangfire job that sends a welcome email via useSend.</summary>
public class EmailJob(IUseSend useSend)
{
    public async Task SendWelcomeAsync(string to, CancellationToken ct)
    {
        var response = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "hello@myapp.com",
            To      = to,
            Subject = "Welcome!",
            Html    = "<h1>Welcome to myapp!</h1><p>Thanks for signing up.</p>"
        }, ct);

        if (!response.Success)
            throw new InvalidOperationException($"useSend error: {response.Exception?.Message}");
    }
}
