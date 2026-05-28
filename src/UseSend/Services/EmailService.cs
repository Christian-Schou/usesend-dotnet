using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using UseSend.Payloads;

namespace UseSend;

/// <summary>
///     Operations on the <c>/v1/emails</c> resource.
/// </summary>
public interface IEmailService
{
    /// <summary>Sends an email.</summary>
    /// <returns>The email identifier.</returns>
    Task<UseSendResponse<string>> SendAsync(EmailMessage email, CancellationToken cancellationToken = default);

    /// <summary>Sends an email with an idempotency key to prevent duplicate sends on retry.</summary>
    /// <returns>The email identifier.</returns>
    Task<UseSendResponse<string>> SendAsync(string idempotencyKey, EmailMessage email,
        CancellationToken cancellationToken = default);

    /// <summary>Retrieves the full detail of a sent email, including delivery events.</summary>
    Task<UseSendResponse<EmailReceipt>> GetAsync(string emailId, CancellationToken cancellationToken = default);

    /// <summary>Lists emails with optional filtering.</summary>
    Task<UseSendResponse<List<EmailListItem>>> ListAsync(EmailListQuery? query = null,
        CancellationToken cancellationToken = default);

    /// <summary>Sends a batch of emails (up to 100).</summary>
    /// <returns>List of email identifiers in the same order as the input.</returns>
    Task<UseSendResponse<List<string>>> BatchAsync(IEnumerable<EmailMessage> emails,
        CancellationToken cancellationToken = default);

    /// <summary>Sends a batch of emails with an idempotency key.</summary>
    /// <returns>List of email identifiers in the same order as the input.</returns>
    Task<UseSendResponse<List<string>>> BatchAsync(string idempotencyKey, IEnumerable<EmailMessage> emails,
        CancellationToken cancellationToken = default);

    /// <summary>Cancels a scheduled email.</summary>
    Task<UseSendResponse> CancelScheduleAsync(string emailId, CancellationToken cancellationToken = default);

    /// <summary>Updates the scheduled send time of an email.</summary>
    Task<UseSendResponse> UpdateScheduleAsync(string emailId, DateTimeOffset scheduledAt,
        CancellationToken cancellationToken = default);
}

internal sealed class EmailService : IEmailService
{
    private readonly HttpExecutor _x;

    internal EmailService(HttpExecutor executor)
    {
        _x = executor;
    }


    /// <inheritdoc />
    public Task<UseSendResponse<string>> SendAsync(EmailMessage email, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/emails");
        req.Content = JsonContent.Create(email, options: HttpExecutor.JsonOptions);

        return _x.Execute<EmailIdPayload, string>(req, x => x.EmailId!, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<string>> SendAsync(string idempotencyKey, EmailMessage email,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/emails");
        req.Content = JsonContent.Create(email, options: HttpExecutor.JsonOptions);
        req.Headers.Add(HttpExecutor.IdempotencyKeyHeader, idempotencyKey);

        return _x.Execute<EmailIdPayload, string>(req, x => x.EmailId!, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<EmailReceipt>> GetAsync(string emailId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"v1/emails/{emailId}");

        return _x.Execute<EmailReceipt, EmailReceipt>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<EmailListItem>>> ListAsync(EmailListQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = "v1/emails";
        var url = baseUrl;

        if (query != null)
        {
            var qs = new Dictionary<string, string?>();

            if (query.Page.HasValue)
                qs.Add("page", query.Page.Value.ToString());

            if (query.Limit.HasValue)
                qs.Add("limit", query.Limit.Value.ToString());

            if (query.StartDate.HasValue)
                qs.Add("startDate", query.StartDate.Value.ToString("o"));

            if (query.EndDate.HasValue)
                qs.Add("endDate", query.EndDate.Value.ToString("o"));

            if (query.DomainId != null)
                qs.Add("domainId", query.DomainId.ToString());

            url = QueryHelpers.AddQueryString(baseUrl, qs);
        }

        var req = new HttpRequestMessage(HttpMethod.Get, url);

        return _x.Execute<DataListWithCount<EmailListItem>, List<EmailListItem>>(req, x => x.Data, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<string>>> BatchAsync(IEnumerable<EmailMessage> emails,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/emails/batch");
        req.Content = JsonContent.Create(emails, options: HttpExecutor.JsonOptions);

        return _x.Execute<DataList<EmailIdPayload>, List<string>>(req, x => x.Data.Select(e => e.EmailId!).ToList(),
            cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<string>>> BatchAsync(string idempotencyKey, IEnumerable<EmailMessage> emails,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/emails/batch");
        req.Content = JsonContent.Create(emails, options: HttpExecutor.JsonOptions);
        req.Headers.Add(HttpExecutor.IdempotencyKeyHeader, idempotencyKey);

        return _x.Execute<DataList<EmailIdPayload>, List<string>>(req, x => x.Data.Select(e => e.EmailId!).ToList(),
            cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> CancelScheduleAsync(string emailId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"v1/emails/{emailId}/cancel");

        return _x.Execute(req, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> UpdateScheduleAsync(string emailId, DateTimeOffset scheduledAt,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Patch, $"v1/emails/{emailId}");
        req.Content = JsonContent.Create(new { scheduledAt }, options: HttpExecutor.JsonOptions);

        return _x.Execute(req, cancellationToken);
    }
}