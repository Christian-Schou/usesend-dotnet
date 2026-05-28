using Xunit;

namespace UseSend.Identity.Tests;

public class UseSendEmailSenderTests
{
    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    private static (UseSendEmailSender sender, FakeEmailService fake) Create(
        string from = "noreply@example.com", string? fromName = null)
    {
        var fake = new FakeEmailService();
        var options = new EmailSenderOptions(from, fromName);
        return (new UseSendEmailSender(fake, options), fake);
    }

    private static (UseSendEmailSender<FakeUser> sender, FakeEmailService fake) CreateTyped(
        string from = "noreply@example.com", string? fromName = null)
    {
        var fake = new FakeEmailService();
        var options = new EmailSenderOptions(from, fromName);
        return (new UseSendEmailSender<FakeUser>(fake, options), fake);
    }

    // -----------------------------------------------------------------------
    // IEmailSender tests
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SendEmailAsync_MapsToAndSubjectCorrectly()
    {
        var (sender, fake) = Create();

        await sender.SendEmailAsync("user@example.com", "Hello", "<p>Hi</p>");

        Assert.Equal("user@example.com", fake.LastMessage!.To);
        Assert.Equal("Hello", fake.LastMessage.Subject);
        Assert.Equal("<p>Hi</p>", fake.LastMessage.Html);
    }

    [Fact]
    public async Task SendEmailAsync_UsesPlainFromAddress_WhenNoName()
    {
        var (sender, fake) = Create("noreply@example.com");

        await sender.SendEmailAsync("user@example.com", "Subject", "<p>Body</p>");

        Assert.Equal("noreply@example.com", fake.LastMessage!.From);
    }

    [Fact]
    public async Task SendEmailAsync_FormatsFromWithDisplayName()
    {
        var (sender, fake) = Create("noreply@example.com", "My App");

        await sender.SendEmailAsync("user@example.com", "Subject", "<p>Body</p>");

        Assert.Equal("My App <noreply@example.com>", fake.LastMessage!.From);
    }

    // -----------------------------------------------------------------------
    // IEmailSender<TUser> tests
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SendConfirmationLinkAsync_SendsEmailWithLink()
    {
        var (sender, fake) = CreateTyped();
        var user = new FakeUser("alice");

        await sender.SendConfirmationLinkAsync(user, "alice@example.com", "https://example.com/confirm?token=abc");

        Assert.Equal("alice@example.com", fake.LastMessage!.To);
        Assert.Equal("Confirm your email address", fake.LastMessage.Subject);
        Assert.Contains("https://example.com/confirm?token=abc", fake.LastMessage.Html!);
    }

    [Fact]
    public async Task SendPasswordResetLinkAsync_SendsEmailWithLink()
    {
        var (sender, fake) = CreateTyped();
        var user = new FakeUser("alice");

        await sender.SendPasswordResetLinkAsync(user, "alice@example.com", "https://example.com/reset?token=xyz");

        Assert.Equal("alice@example.com", fake.LastMessage!.To);
        Assert.Equal("Reset your password", fake.LastMessage.Subject);
        Assert.Contains("https://example.com/reset?token=xyz", fake.LastMessage.Html!);
    }

    [Fact]
    public async Task SendPasswordResetCodeAsync_SendsEmailWithCode()
    {
        var (sender, fake) = CreateTyped();
        var user = new FakeUser("alice");

        await sender.SendPasswordResetCodeAsync(user, "alice@example.com", "123456");

        Assert.Equal("alice@example.com", fake.LastMessage!.To);
        Assert.Equal("Reset your password", fake.LastMessage.Subject);
        Assert.Contains("123456", fake.LastMessage.Html!);
    }

    [Fact]
    public async Task SendConfirmationLinkAsync_UsesFormattedFrom()
    {
        var (sender, fake) = CreateTyped("no-reply@app.com", "Cool App");

        await sender.SendConfirmationLinkAsync(new FakeUser("bob"), "bob@example.com", "https://confirm");

        Assert.Equal("Cool App <no-reply@app.com>", fake.LastMessage!.From);
    }

    // -----------------------------------------------------------------------
    // Fakes
    // -----------------------------------------------------------------------

    private sealed class FakeUser(string name)
    {
        public string UserName { get; } = name;
    }

    private sealed class FakeEmailService : IEmailService
    {
        public EmailMessage? LastMessage { get; private set; }

        public Task<UseSendResponse<string>> SendAsync(EmailMessage email, CancellationToken ct = default)
        {
            LastMessage = email;
            return Task.FromResult(new UseSendResponse<string>("fake-id"));
        }

        public Task<UseSendResponse<string>> SendAsync(string idempotencyKey, EmailMessage email, CancellationToken ct = default)
            => SendAsync(email, ct);

        public Task<UseSendResponse<EmailReceipt>> GetAsync(string emailId, CancellationToken ct = default)
            => throw new NotImplementedException();

        public Task<UseSendResponse<List<EmailListItem>>> ListAsync(EmailListQuery? query = null, CancellationToken ct = default)
            => throw new NotImplementedException();

        public Task<UseSendResponse<List<string>>> BatchAsync(IEnumerable<EmailMessage> emails, CancellationToken ct = default)
            => throw new NotImplementedException();

        public Task<UseSendResponse<List<string>>> BatchAsync(string idempotencyKey, IEnumerable<EmailMessage> emails, CancellationToken ct = default)
            => throw new NotImplementedException();

        public Task<UseSendResponse> CancelScheduleAsync(string emailId, CancellationToken ct = default)
            => throw new NotImplementedException();

        public Task<UseSendResponse> UpdateScheduleAsync(string emailId, DateTimeOffset scheduledAt, CancellationToken ct = default)
            => throw new NotImplementedException();
    }
}
