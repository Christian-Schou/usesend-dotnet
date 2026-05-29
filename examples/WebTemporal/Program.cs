using Temporalio.Activities;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;
using Temporalio.Workflows;
using UseSend;

var builder = Host.CreateApplicationBuilder(args);

// --- useSend ---
builder.Services.AddUseSend(builder.Configuration["UseSend:ApiToken"]
    ?? throw new InvalidOperationException("UseSend:ApiToken not configured."));

// --- Temporal worker (connects to local dev server on localhost:7233 by default) ---
builder.Services.AddTemporalClient(opts =>
{
    opts.TargetHost = builder.Configuration["Temporal:TargetHost"] ?? "localhost:7233";
});

builder.Services.AddHostedTemporalWorker("email-task-queue")
    .AddWorkflow<WelcomeEmailWorkflow>()
    .AddScopedActivities<EmailActivities>();

var host = builder.Build();
host.Run();

// ---------- Workflow ----------

[Workflow]
public class WelcomeEmailWorkflow
{
    [WorkflowRun]
    public async Task RunAsync(WelcomeEmailParams input)
    {
        // Send the welcome email
        await Workflow.ExecuteActivityAsync(
            (EmailActivities a) => a.SendWelcomeEmailAsync(input),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(2) });

        // Wait 3 days, then send a follow-up (shows durable timer)
        await Workflow.DelayAsync(TimeSpan.FromDays(3));

        await Workflow.ExecuteActivityAsync(
            (EmailActivities a) => a.SendFollowUpEmailAsync(input.To),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(2) });
    }
}

// ---------- Activities ----------

public class EmailActivities(IUseSend useSend)
{
    [Activity]
    public async Task SendWelcomeEmailAsync(WelcomeEmailParams input)
    {
        var response = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "hello@myapp.com",
            To      = input.To,
            Subject = $"Welcome, {input.Name}!",
            Html    = $"<h1>Hi {input.Name}!</h1><p>Thanks for joining us.</p>"
        });

        if (!response.Success)
            throw new ApplicationException($"useSend error: {response.Exception?.Message}");
    }

    [Activity]
    public async Task SendFollowUpEmailAsync(string to)
    {
        var response = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "hello@myapp.com",
            To      = to,
            Subject = "How is everything going?",
            Html    = "<p>Just checking in! Let us know if you need anything.</p>"
        });

        if (!response.Success)
            throw new ApplicationException($"useSend error: {response.Exception?.Message}");
    }
}

// ---------- Params ----------

public record WelcomeEmailParams(string To, string Name);
