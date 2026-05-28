using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UseSend;
using UseSend.Identity;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// Database (InMemory — swap for SQL Server / PostgreSQL in production)
// -----------------------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("WebIdentity"));

// -----------------------------------------------------------------------
// Identity
// -----------------------------------------------------------------------
builder.Services
    .AddIdentityCore<AppUser>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// -----------------------------------------------------------------------
// useSend
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

// Register the useSend Identity email sender.
// Swap AddUseSendIdentityEmailSender<AppUser> with a custom subclass to override email templates.
builder.Services.AddUseSendIdentityEmailSender<AppUser>(
    fromAddress: builder.Configuration["UseSend:FromAddress"] ?? "noreply@example.com",
    fromName: builder.Configuration["UseSend:FromName"] ?? "My App"
);

// -----------------------------------------------------------------------
// Minimal API endpoints
// -----------------------------------------------------------------------
var app = builder.Build();

// POST /register — creates a user and sends a confirmation email
app.MapPost("/register", async (RegisterRequest req, UserManager<AppUser> users,
    IEmailSender<AppUser> emailSender, LinkGenerator links, HttpContext ctx) =>
{
    var user = new AppUser { UserName = req.Email, Email = req.Email };
    var result = await users.CreateAsync(user, req.Password);

    if (!result.Succeeded)
        return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));

    var token = await users.GenerateEmailConfirmationTokenAsync(user);
    var confirmUrl = links.GetUriByName(ctx, "ConfirmEmail",
        new { userId = user.Id, token }) ?? "#";

    await emailSender.SendConfirmationLinkAsync(user, user.Email!, confirmUrl);

    return Results.Ok(new { message = "Registration successful — check your email to confirm your account." });
});

// GET /confirm-email — confirms the user's email address
app.MapGet("/confirm-email", async (string userId, string token, UserManager<AppUser> users) =>
{
    var user = await users.FindByIdAsync(userId);
    if (user is null) return Results.NotFound();

    var result = await users.ConfirmEmailAsync(user, token);
    return result.Succeeded
        ? Results.Ok(new { message = "Email confirmed. You can now log in." })
        : Results.BadRequest(result.Errors.Select(e => e.Description));
}).WithName("ConfirmEmail");

// POST /forgot-password — sends a password reset link
app.MapPost("/forgot-password", async (ForgotPasswordRequest req, UserManager<AppUser> users,
    IEmailSender<AppUser> emailSender, LinkGenerator links, HttpContext ctx) =>
{
    var user = await users.FindByEmailAsync(req.Email);

    // Always return 200 to avoid email enumeration
    if (user is null || !await users.IsEmailConfirmedAsync(user))
        return Results.Ok(new { message = "If that email is registered, a reset link has been sent." });

    var token = await users.GeneratePasswordResetTokenAsync(user);
    var resetUrl = links.GetUriByName(ctx, "ResetPassword",
        new { userId = user.Id, token }) ?? "#";

    await emailSender.SendPasswordResetLinkAsync(user, user.Email!, resetUrl);

    return Results.Ok(new { message = "If that email is registered, a reset link has been sent." });
});

// POST /reset-password — resets the user's password using the token
app.MapPost("/reset-password", async (ResetPasswordRequest req, UserManager<AppUser> users) =>
{
    var user = await users.FindByIdAsync(req.UserId);
    if (user is null) return Results.BadRequest("Invalid request.");

    var result = await users.ResetPasswordAsync(user, req.Token, req.NewPassword);
    return result.Succeeded
        ? Results.Ok(new { message = "Password reset successfully." })
        : Results.BadRequest(result.Errors.Select(e => e.Description));
}).WithName("ResetPassword");

app.Run();

// -----------------------------------------------------------------------
// Supporting types
// -----------------------------------------------------------------------

public class AppUser : IdentityUser { }

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser>(options);

internal record RegisterRequest(string Email, string Password);
internal record ForgotPasswordRequest(string Email);
internal record ResetPasswordRequest(string UserId, string Token, string NewPassword);
