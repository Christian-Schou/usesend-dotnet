using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace UseSend.Webhooks;

/// <summary>
///     Verifies and parses incoming useSend webhook requests.
/// </summary>
public class UseSendWebhooks
{
    private const string SignatureHeader = "X-UseSend-Signature";
    private const string TimestampHeader = "X-UseSend-Timestamp";
    private const string SignaturePrefix = "v1=";

    private static readonly TimeSpan Tolerance = TimeSpan.FromMinutes(5);

    private readonly byte[] _secretBytes;


    /// <summary>
    ///     Initializes a new instance of <see cref="UseSendWebhooks" />.
    /// </summary>
    /// <param name="signingSecret">The webhook signing secret from your useSend dashboard.</param>
    public UseSendWebhooks(string signingSecret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(signingSecret);
        _secretBytes = Encoding.UTF8.GetBytes(signingSecret);
    }


    /// <summary>
    ///     Verifies the webhook signature without parsing the event.
    /// </summary>
    /// <param name="rawBody">The raw (unmodified) request body string.</param>
    /// <param name="headers">A dictionary of request headers (case-insensitive lookup).</param>
    /// <returns><c>true</c> if the signature is valid and the timestamp is within the tolerance window.</returns>
    public bool Verify(string rawBody, IDictionary<string, string> headers)
    {
        try
        {
            VerifyOrThrow(rawBody, headers);
            return true;
        }
        catch (WebhookException)
        {
            return false;
        }
    }


    /// <summary>
    ///     Verifies the signature and parses the webhook payload into a <see cref="WebhookEvent" />.
    /// </summary>
    /// <param name="rawBody">The raw (unmodified) request body string.</param>
    /// <param name="headers">A dictionary of request headers (case-insensitive lookup).</param>
    /// <returns>The parsed <see cref="WebhookEvent" />.</returns>
    /// <exception cref="WebhookException">Thrown when signature is invalid, timestamp is missing or stale.</exception>
    public WebhookEvent ConstructEvent(string rawBody, IDictionary<string, string> headers)
    {
        VerifyOrThrow(rawBody, headers);

        WebhookEvent? evt;

        try
        {
            evt = JsonSerializer.Deserialize<WebhookEvent>(rawBody, WebhookJsonOptions.Default);
        }
        catch (JsonException ex)
        {
            throw new WebhookException($"Failed to parse webhook payload: {ex.Message}");
        }

        return evt ?? throw new WebhookException("Webhook payload was empty or null.");
    }


    private void VerifyOrThrow(string rawBody, IDictionary<string, string> headers)
    {
        var lookup = new Dictionary<string, string>(headers, StringComparer.OrdinalIgnoreCase);

        if (!lookup.TryGetValue(TimestampHeader, out var timestampStr) || string.IsNullOrEmpty(timestampStr))
            throw new WebhookException($"Missing {TimestampHeader} header.");

        if (!long.TryParse(timestampStr, out var timestampMs))
            throw new WebhookException($"Invalid {TimestampHeader} header value.");

        var eventTime = DateTimeOffset.FromUnixTimeMilliseconds(timestampMs);
        if (DateTimeOffset.UtcNow - eventTime > Tolerance)
            throw new WebhookException("Webhook timestamp is too old — possible replay attack.");

        if (!lookup.TryGetValue(SignatureHeader, out var signatureHeader) || string.IsNullOrEmpty(signatureHeader))
            throw new WebhookException($"Missing {SignatureHeader} header.");

        if (!signatureHeader.StartsWith(SignaturePrefix, StringComparison.Ordinal))
            throw new WebhookException($"Unsupported signature format (expected '{SignaturePrefix}...').");

        var receivedHex = signatureHeader[SignaturePrefix.Length..];

        var payload = Encoding.UTF8.GetBytes($"{timestampStr}.{rawBody}");
        var expectedHex = ComputeHmacSha256Hex(payload);

        if (!ConstantTimeEquals(expectedHex, receivedHex))
            throw new WebhookException("Webhook signature verification failed.");
    }


    private string ComputeHmacSha256Hex(byte[] payload)
    {
        using var hmac = new HMACSHA256(_secretBytes);
        var hash = hmac.ComputeHash(payload);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }


    private static bool ConstantTimeEquals(string a, string b)
    {
        var aBytes = Encoding.UTF8.GetBytes(a);
        var bBytes = Encoding.UTF8.GetBytes(b);
        return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
    }
}