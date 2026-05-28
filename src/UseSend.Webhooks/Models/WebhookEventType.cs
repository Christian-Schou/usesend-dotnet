namespace UseSend.Webhooks;

/// <summary>
///     String constants for all useSend webhook event types.
/// </summary>
#pragma warning disable CS1591
public static class WebhookEventType
{
    // Email events
    public const string EmailQueued = "email.queued";
    public const string EmailSent = "email.sent";
    public const string EmailDelivered = "email.delivered";
    public const string EmailDeliveryDelayed = "email.delivery_delayed";
    public const string EmailBounced = "email.bounced";
    public const string EmailRejected = "email.rejected";
    public const string EmailRenderingFailure = "email.rendering_failure";
    public const string EmailComplained = "email.complained";
    public const string EmailFailed = "email.failed";
    public const string EmailCancelled = "email.cancelled";
    public const string EmailSuppressed = "email.suppressed";
    public const string EmailOpened = "email.opened";
    public const string EmailClicked = "email.clicked";

    // Contact events
    public const string ContactCreated = "contact.created";
    public const string ContactUpdated = "contact.updated";
    public const string ContactDeleted = "contact.deleted";

    // Domain events
    public const string DomainCreated = "domain.created";
    public const string DomainVerified = "domain.verified";
    public const string DomainUpdated = "domain.updated";
    public const string DomainDeleted = "domain.deleted";

    // Test event (sent from dashboard)
    public const string WebhookTest = "webhook.test";
}
#pragma warning restore CS1591