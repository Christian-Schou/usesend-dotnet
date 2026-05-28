using System.Security.Cryptography;
using System.Text;

namespace UseSend.Webhooks.Tests;

public class UseSendWebhooksTests
{
    private const string Secret = "wh_test_secret";

    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    private static Dictionary<string, string> BuildHeaders(string rawBody, string secret,
        long? timestampOverrideMs = null)
    {
        var tsMs = timestampOverrideMs ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var payload = Encoding.UTF8.GetBytes($"{tsMs}.{rawBody}");
        var secretKey = Encoding.UTF8.GetBytes(secret);

        using var hmac = new HMACSHA256(secretKey);
        var hex = Convert.ToHexString(hmac.ComputeHash(payload)).ToLowerInvariant();

        return new Dictionary<string, string>
        {
            { "X-UseSend-Signature", $"v1={hex}" },
            { "X-UseSend-Timestamp", tsMs.ToString() }
        };
    }

    private static string EmailDeliveredPayload(string id = "email_abc")
    {
        return $$"""
                 {
                   "id": "call_123",
                   "type": "email.delivered",
                   "version": "2026-01-18",
                   "createdAt": "2024-01-15T10:30:00.000Z",
                   "teamId": 42,
                   "data": {
                     "id": "{{id}}",
                     "status": "DELIVERED",
                     "from": "sender@example.com",
                     "to": ["recipient@example.com"],
                     "subject": "Hello",
                     "occurredAt": "2024-01-15T10:30:00Z"
                   },
                   "attempt": 1
                 }
                 """;
    }


    // -----------------------------------------------------------------------
    // Verify()
    // -----------------------------------------------------------------------

    [Fact]
    public void Verify_ValidSignature_ReturnsTrue()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, Secret);

        Assert.True(webhooks.Verify(body, headers));
    }


    [Fact]
    public void Verify_WrongSecret_ReturnsFalse()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, "wrong_secret");

        Assert.False(webhooks.Verify(body, headers));
    }


    [Fact]
    public void Verify_TamperedBody_ReturnsFalse()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, Secret);

        Assert.False(webhooks.Verify(body + " ", headers));
    }


    [Fact]
    public void Verify_StaleTimestamp_ReturnsFalse()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var staleTs = DateTimeOffset.UtcNow.AddMinutes(-6).ToUnixTimeMilliseconds();
        var headers = BuildHeaders(body, Secret, staleTs);

        Assert.False(webhooks.Verify(body, headers));
    }


    [Fact]
    public void Verify_MissingSignatureHeader_ReturnsFalse()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, Secret);
        headers.Remove("X-UseSend-Signature");

        Assert.False(webhooks.Verify(body, headers));
    }


    [Fact]
    public void Verify_MissingTimestampHeader_ReturnsFalse()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, Secret);
        headers.Remove("X-UseSend-Timestamp");

        Assert.False(webhooks.Verify(body, headers));
    }


    // -----------------------------------------------------------------------
    // ConstructEvent()
    // -----------------------------------------------------------------------

    [Fact]
    public void ConstructEvent_ValidPayload_ParsesEnvelope()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, Secret);

        var evt = webhooks.ConstructEvent(body, headers);

        Assert.Equal("call_123", evt.Id);
        Assert.Equal("email.delivered", evt.Type);
        Assert.Equal("2026-01-18", evt.Version);
        Assert.Equal(42L, evt.TeamId);
        Assert.Equal(1, evt.Attempt);
    }


    [Fact]
    public void ConstructEvent_InvalidSignature_ThrowsWebhookException()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, "wrong_secret");

        Assert.Throws<WebhookException>(() => webhooks.ConstructEvent(body, headers));
    }


    [Fact]
    public void ConstructEvent_StaleTimestamp_ThrowsWebhookException()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var headers = BuildHeaders(body, Secret, DateTimeOffset.UtcNow.AddMinutes(-6).ToUnixTimeMilliseconds());

        Assert.Throws<WebhookException>(() => webhooks.ConstructEvent(body, headers));
    }


    // -----------------------------------------------------------------------
    // GetData<T>()
    // -----------------------------------------------------------------------

    [Fact]
    public void GetData_EmailEventData_DeserializesBaseFields()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload("email_xyz");
        var headers = BuildHeaders(body, Secret);

        var evt = webhooks.ConstructEvent(body, headers);
        var data = evt.GetData<EmailEventData>();

        Assert.NotNull(data);
        Assert.Equal("email_xyz", data.Id);
        Assert.Equal("DELIVERED", data.Status);
        Assert.Equal("sender@example.com", data.From);
        Assert.Contains("recipient@example.com", data.To);
        Assert.Equal("Hello", data.Subject);
    }


    [Fact]
    public void GetData_EmailBouncedEventData_DeserializesBounceDetails()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = """
                   {
                     "id": "call_bounce",
                     "type": "email.bounced",
                     "version": "2026-01-18",
                     "createdAt": "2024-01-15T10:30:00.000Z",
                     "teamId": 1,
                     "data": {
                       "id": "email_bounce",
                       "status": "BOUNCED",
                       "from": "f@e.com",
                       "to": ["r@e.com"],
                       "occurredAt": "2024-01-15T10:30:00Z",
                       "bounce": {
                         "type": "Permanent",
                         "subType": "NoEmail",
                         "message": "User does not exist"
                       }
                     },
                     "attempt": 1
                   }
                   """;
        var headers = BuildHeaders(body, Secret);

        var evt = webhooks.ConstructEvent(body, headers);
        var data = evt.GetData<EmailBouncedEventData>();

        Assert.NotNull(data?.Bounce);
        Assert.Equal("Permanent", data.Bounce.Type);
        Assert.Equal("NoEmail", data.Bounce.SubType);
        Assert.Equal("User does not exist", data.Bounce.Message);
    }


    [Fact]
    public void GetData_ContactEventData_DeserializesCorrectly()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = """
                   {
                     "id": "call_contact",
                     "type": "contact.created",
                     "version": "2026-01-18",
                     "createdAt": "2024-01-15T10:30:00.000Z",
                     "teamId": 1,
                     "data": {
                       "id": "c_123",
                       "email": "user@example.com",
                       "contactBookId": "cb_456",
                       "subscribed": true,
                       "firstName": "Jane",
                       "lastName": "Doe",
                       "createdAt": "2024-01-01T00:00:00Z",
                       "updatedAt": "2024-01-15T00:00:00Z"
                     },
                     "attempt": 1
                   }
                   """;
        var headers = BuildHeaders(body, Secret);

        var evt = webhooks.ConstructEvent(body, headers);
        var data = evt.GetData<ContactEventData>();

        Assert.NotNull(data);
        Assert.Equal("c_123", data.Id);
        Assert.Equal("user@example.com", data.Email);
        Assert.Equal("cb_456", data.ContactBookId);
        Assert.True(data.Subscribed);
        Assert.Equal("Jane", data.FirstName);
        Assert.Equal("Doe", data.LastName);
    }


    [Fact]
    public void GetData_DomainEventData_DeserializesCorrectly()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = """
                   {
                     "id": "call_domain",
                     "type": "domain.verified",
                     "version": "2026-01-18",
                     "createdAt": "2024-01-15T10:30:00.000Z",
                     "teamId": 1,
                     "data": {
                       "id": 7,
                       "name": "example.com",
                       "status": "VERIFIED",
                       "region": "us-east-1",
                       "clickTracking": true,
                       "openTracking": false,
                       "createdAt": "2024-01-01T00:00:00Z",
                       "updatedAt": "2024-01-15T00:00:00Z"
                     },
                     "attempt": 1
                   }
                   """;
        var headers = BuildHeaders(body, Secret);

        var evt = webhooks.ConstructEvent(body, headers);
        var data = evt.GetData<DomainEventData>();

        Assert.NotNull(data);
        Assert.Equal(7L, data.Id);
        Assert.Equal("example.com", data.Name);
        Assert.Equal("VERIFIED", data.Status);
        Assert.Equal("us-east-1", data.Region);
        Assert.True(data.ClickTracking);
        Assert.False(data.OpenTracking);
    }


    [Fact]
    public void GetData_WebhookTestData_DeserializesCorrectly()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = """
                   {
                     "id": "call_test",
                     "type": "webhook.test",
                     "version": "2026-01-18",
                     "createdAt": "2024-01-15T10:30:00.000Z",
                     "teamId": 1,
                     "data": {
                       "test": true,
                       "webhookId": "wh_abc123",
                       "sentAt": "2024-01-15T10:30:00.000Z"
                     },
                     "attempt": 1
                   }
                   """;
        var headers = BuildHeaders(body, Secret);

        var evt = webhooks.ConstructEvent(body, headers);

        Assert.Equal(WebhookEventType.WebhookTest, evt.Type);

        var data = evt.GetData<WebhookTestData>();
        Assert.NotNull(data);
        Assert.True(data.Test);
        Assert.Equal("wh_abc123", data.WebhookId);
    }


    // -----------------------------------------------------------------------
    // Header lookup is case-insensitive
    // -----------------------------------------------------------------------

    [Fact]
    public void Verify_HeaderLookup_IsCaseInsensitive()
    {
        var webhooks = new UseSendWebhooks(Secret);
        var body = EmailDeliveredPayload();
        var tsMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var payload = Encoding.UTF8.GetBytes($"{tsMs}.{body}");

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret));
        var hex = Convert.ToHexString(hmac.ComputeHash(payload)).ToLowerInvariant();

        var headers = new Dictionary<string, string>
        {
            { "x-usesend-signature", $"v1={hex}" },
            { "x-usesend-timestamp", tsMs.ToString() }
        };

        Assert.True(webhooks.Verify(body, headers));
    }
}