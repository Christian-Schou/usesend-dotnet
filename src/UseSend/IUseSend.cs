namespace UseSend;

/// <summary>
///     Entry point for the useSend client. Provides access to all API resource services.
/// </summary>
public interface IUseSend
{
    /// <summary>Email sending and management operations.</summary>
    IEmailService Emails { get; }

    /// <summary>Domain registration and verification operations.</summary>
    IDomainService Domains { get; }

    /// <summary>Contact management operations (scoped to a contact book).</summary>
    IContactService Contacts { get; }

    /// <summary>Contact book management operations.</summary>
    IContactBookService ContactBooks { get; }

    /// <summary>Campaign creation, scheduling, and lifecycle operations.</summary>
    ICampaignService Campaigns { get; }

    /// <summary>Email delivery and reputation analytics operations.</summary>
    IAnalyticsService Analytics { get; }
}