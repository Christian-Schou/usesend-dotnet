using UseSend;

var builder = WebApplication.CreateBuilder(args);

// Register useSend — reads config from appsettings.json or environment variables.
// For a self-hosted instance, set "UseSend:ApiUrl" in appsettings.json.
builder.Services.AddUseSend(options =>
{
    options.ApiToken = builder.Configuration["UseSend:ApiToken"]
                       ?? Environment.GetEnvironmentVariable("USESEND_API_KEY")
                       ?? throw new InvalidOperationException("UseSend API token is required.");

    var apiUrl = builder.Configuration["UseSend:ApiUrl"];
    if (!string.IsNullOrWhiteSpace(apiUrl))
        options.ApiUrl = apiUrl;
});

var app = builder.Build();

// POST /send  — sends a transactional email
app.MapPost("/send", async (SendRequest body, IUseSend useSend) =>
{
    var response = await useSend.Emails.SendAsync(new EmailMessage
    {
        From = body.From,
        To = body.To,
        Subject = body.Subject,
        Html = body.Html,
        Text = body.Text
    });

    return response.Success
        ? Results.Ok(new { emailId = response.Content })
        : Results.Problem(response.Exception?.ApiError ?? "Unknown error",
            statusCode: response.Exception?.StatusCode ?? 500);
});

// GET /domains — lists verified domains
app.MapGet("/domains", async (IUseSend useSend) =>
{
    var response = await useSend.Domains.ListAsync();

    return response.Success
        ? Results.Ok(response.Content)
        : Results.Problem(response.Exception?.ApiError ?? "Unknown error",
            statusCode: response.Exception?.StatusCode ?? 500);
});

app.Run();


internal record SendRequest(string From, string To, string Subject, string? Html, string? Text);