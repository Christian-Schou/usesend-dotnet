using FluentEmail.Core;

namespace UseSend.FluentEmail.Tests;

public class UseSendSenderTests
{
    private static List<string> ToList(object? to)
    {
        return Assert.IsType<List<string>>(to);
    }


    // -----------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------

    [Fact]
    public async Task SendAsync_Success_ReturnsSendResponseWithMessageId()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        var email = new Email("from@example.com")
            .To("to@example.com")
            .Subject("Test")
            .Body("<p>Hi</p>", true);

        var response = await sender.SendAsync(email);

        Assert.True(response.Successful);
        Assert.Equal("msg_abc123", response.MessageId);
    }


    [Fact]
    public async Task SendAsync_MapsFrom_Correctly()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com").To("to@example.com").Subject("S").Body("B"));

        Assert.Equal("from@example.com", fake.LastSent!.From);
    }


    [Fact]
    public async Task SendAsync_MapsTo_Correctly()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com").To("to@example.com").Subject("S").Body("B"));

        Assert.Contains("to@example.com", ToList(fake.LastSent!.To));
    }


    [Fact]
    public async Task SendAsync_MapsSubject_Correctly()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com").To("to@example.com").Subject("My Subject").Body("B"));

        Assert.Equal("My Subject", fake.LastSent!.Subject);
    }


    [Fact]
    public async Task SendAsync_HtmlBody_MapsToHtml()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com").To("to@example.com").Subject("S")
            .Body("<p>Hello</p>", true));

        Assert.Equal("<p>Hello</p>", fake.LastSent!.Html);
        Assert.Null(fake.LastSent.Text);
    }


    [Fact]
    public async Task SendAsync_PlainTextBody_MapsToText()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(
            new Email("from@example.com").To("to@example.com").Subject("S").Body("Plain text", false));

        Assert.Equal("Plain text", fake.LastSent!.Text);
        Assert.Null(fake.LastSent.Html);
    }


    [Fact]
    public async Task SendAsync_NamedAddresses_FormatCorrectly()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com", "Sender Name").To("to@example.com", "Recipient")
            .Subject("S").Body("B"));

        Assert.Equal("Sender Name <from@example.com>", fake.LastSent!.From);
        Assert.Contains("Recipient <to@example.com>", ToList(fake.LastSent.To));
    }


    [Fact]
    public async Task SendAsync_ApiError_ReturnsSendResponseWithError()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);
        fake.NextResponse = new UseSendResponse<string>(new UseSendException(422, "Invalid from", "Error"));

        var response = await sender.SendAsync(new Email("bad").To("to@example.com").Subject("S").Body("B"));

        Assert.False(response.Successful);
        Assert.NotEmpty(response.ErrorMessages);
    }


    [Fact]
    public async Task SendAsync_CC_MapsCorrectly()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com").To("to@example.com").CC("cc@example.com").Subject("S")
            .Body("B"));

        Assert.NotNull(fake.LastSent!.Cc);
        Assert.Contains("cc@example.com", ToList(fake.LastSent.Cc));
    }


    [Fact]
    public async Task SendAsync_BCC_MapsCorrectly()
    {
        var fake = new FakeEmailService();
        var sender = new UseSendSender(fake);

        await sender.SendAsync(new Email("from@example.com").To("to@example.com").BCC("bcc@example.com").Subject("S")
            .Body("B"));

        Assert.NotNull(fake.LastSent!.Bcc);
        Assert.Contains("bcc@example.com", ToList(fake.LastSent.Bcc));
    }
    // -----------------------------------------------------------------------
    // Fake IEmailService
    // -----------------------------------------------------------------------

    private sealed class FakeEmailService : IEmailService
    {
        public EmailMessage? LastSent { get; private set; }
        public UseSendResponse<string> NextResponse { get; set; } = new("msg_abc123");

        public Task<UseSendResponse<string>> SendAsync(EmailMessage email, CancellationToken ct = default)
        {
            LastSent = email;
            return Task.FromResult(NextResponse);
        }

        public Task<UseSendResponse<string>> SendAsync(string idempotencyKey, EmailMessage email,
            CancellationToken ct = default)
        {
            return SendAsync(email, ct);
        }

        public Task<UseSendResponse<EmailReceipt>> GetAsync(string emailId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UseSendResponse<List<EmailListItem>>> ListAsync(EmailListQuery? query = null,
            CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UseSendResponse<List<string>>> BatchAsync(IEnumerable<EmailMessage> emails,
            CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UseSendResponse<List<string>>> BatchAsync(string idempotencyKey, IEnumerable<EmailMessage> emails,
            CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UseSendResponse> CancelScheduleAsync(string emailId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<UseSendResponse> UpdateScheduleAsync(string emailId, DateTimeOffset scheduledAt,
            CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}