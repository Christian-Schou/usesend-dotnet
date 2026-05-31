using UseSend;
using UseSend.Fluid;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// useSend — core SDK
// -----------------------------------------------------------------------
builder.Services.AddUseSend(options =>
{
    options.ApiToken = builder.Configuration["UseSend:ApiToken"]
                       ?? Environment.GetEnvironmentVariable("USESEND_API_KEY")
                       ?? throw new InvalidOperationException("UseSend API token is required.");

    var apiUrl = builder.Configuration["UseSend:ApiUrl"];
    if (!string.IsNullOrWhiteSpace(apiUrl))
        options.ApiUrl = apiUrl;
});

// -----------------------------------------------------------------------
// UseSend.Fluid — Liquid template renderer
// Templates live in wwwroot/../Templates/Emails/*.liquid
// -----------------------------------------------------------------------
builder.Services.AddUseSendFluid(opts =>
    opts.TemplateRootPath = Path.Combine(builder.Environment.ContentRootPath, "Templates"));

// -----------------------------------------------------------------------
// App config helpers
// -----------------------------------------------------------------------
var fromAddress = builder.Configuration["UseSend:FromAddress"] ?? "noreply@example.com";
var fromName    = builder.Configuration["UseSend:FromName"]    ?? "My App";
var appName     = builder.Configuration["App:Name"]            ?? "My App";

var app = builder.Build();

// -----------------------------------------------------------------------
// POST /send-welcome
// Body: { "email": "alice@example.com", "name": "Alice", "confirmUrl": "https://..." }
// -----------------------------------------------------------------------
app.MapPost("/send-welcome", async (
    WelcomeRequest req,
    IUseSend useSend,
    IEmailTemplateRenderer renderer) =>
{
    var html = await renderer.RenderAsync("Emails/Welcome", new
    {
        req.Name,
        req.ConfirmUrl,
        AppName    = appName,
        HasPromo   = req.PromoCode is not null,
        PromoCode  = req.PromoCode
    });

    var result = await useSend.Emails.SendAsync(new EmailMessage
    {
        From    = $"{fromName} <{fromAddress}>",
        To      = req.Email,
        Subject = $"Welcome to {appName}, {req.Name}!",
        Html    = html
    });

    return result.Success
        ? Results.Ok(new { message = "Welcome email sent.", id = result.Content })
        : Results.Problem("Failed to send welcome email.");
});

// -----------------------------------------------------------------------
// POST /send-password-reset
// Body: { "email": "alice@example.com", "name": "Alice", "resetUrl": "https://..." }
// -----------------------------------------------------------------------
app.MapPost("/send-password-reset", async (
    PasswordResetRequest req,
    IUseSend useSend,
    IEmailTemplateRenderer renderer) =>
{
    var html = await renderer.RenderAsync("Emails/PasswordReset", new
    {
        req.Name,
        req.ResetUrl,
        ExpiresInMinutes = 30
    });

    var result = await useSend.Emails.SendAsync(new EmailMessage
    {
        From    = $"{fromName} <{fromAddress}>",
        To      = req.Email,
        Subject = "Reset your password",
        Html    = html
    });

    return result.Success
        ? Results.Ok(new { message = "Password reset email sent.", id = result.Content })
        : Results.Problem("Failed to send password reset email.");
});

app.Run();

// -----------------------------------------------------------------------
// Request models
// -----------------------------------------------------------------------
internal record WelcomeRequest(
    string Email,
    string Name,
    string ConfirmUrl,
    string? PromoCode = null);

internal record PasswordResetRequest(
    string Email,
    string Name,
    string ResetUrl);
