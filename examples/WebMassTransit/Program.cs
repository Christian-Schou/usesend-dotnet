using MassTransit;
using UseSend;

var builder = WebApplication.CreateBuilder(args);

// --- useSend ---
builder.Services.AddUseSend(builder.Configuration["UseSend:ApiToken"]
    ?? throw new InvalidOperationException("UseSend:ApiToken not configured."));

// --- MassTransit (in-memory transport for demo; swap for RabbitMQ/Azure Service Bus/etc.) ---
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendEmailCommandConsumer>();
    x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
});

var app = builder.Build();

// Publish a SendEmailCommand from an HTTP endpoint
app.MapPost("/send", async (SendEmailRequest req, IPublishEndpoint publish) =>
{
    await publish.Publish(new SendEmailCommand(req.To, req.Subject, req.HtmlBody));
    return Results.Accepted();
});

app.Run();

// ---------- Messages ----------

/// <summary>Command message that requests an email to be sent.</summary>
public record SendEmailCommand(string To, string Subject, string HtmlBody);

// ---------- Consumer ----------

/// <summary>MassTransit consumer that handles <see cref="SendEmailCommand" /> messages.</summary>
public class SendEmailCommandConsumer(IUseSend useSend) : IConsumer<SendEmailCommand>
{
    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        var msg = context.Message;
        var response = await useSend.Emails.SendAsync(new EmailMessage
        {
            From    = "hello@myapp.com",
            To      = msg.To,
            Subject = msg.Subject,
            Html    = msg.HtmlBody
        }, context.CancellationToken);

        if (!response.Success)
            throw new InvalidOperationException($"useSend error: {response.Exception?.Message}");
    }
}

// ---------- HTTP request DTO ----------
public record SendEmailRequest(string To, string Subject, string HtmlBody);
