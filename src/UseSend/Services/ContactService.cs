using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using UseSend.Payloads;

namespace UseSend;

/// <summary>
///     Operations on the <c>/v1/contactBooks/{id}/contacts</c> resource.
/// </summary>
public interface IContactService
{
    /// <summary>Creates a contact in a contact book.</summary>
    /// <returns>The new contact identifier.</returns>
    Task<UseSendResponse<string>> CreateAsync(string contactBookId, ContactData data,
        CancellationToken cancellationToken = default);

    /// <summary>Retrieves a contact by ID.</summary>
    Task<UseSendResponse<Contact>> GetAsync(string contactBookId, string contactId,
        CancellationToken cancellationToken = default);

    /// <summary>Lists contacts in a contact book.</summary>
    Task<UseSendResponse<List<Contact>>> ListAsync(string contactBookId, ContactListQuery? query = null,
        CancellationToken cancellationToken = default);

    /// <summary>Partially updates a contact.</summary>
    Task<UseSendResponse> UpdateAsync(string contactBookId, string contactId, ContactUpdateData data,
        CancellationToken cancellationToken = default);

    /// <summary>Creates or updates a contact (upsert by email).</summary>
    /// <returns>The contact identifier.</returns>
    Task<UseSendResponse<string>> UpsertAsync(string contactBookId, ContactData data,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a contact.</summary>
    Task<UseSendResponse> DeleteAsync(string contactBookId, string contactId,
        CancellationToken cancellationToken = default);

    /// <summary>Bulk-creates up to 1000 contacts in a contact book.</summary>
    Task<UseSendResponse<BulkCreateResult>> BulkCreateAsync(string contactBookId, IEnumerable<ContactData> contacts,
        CancellationToken cancellationToken = default);

    /// <summary>Bulk-deletes contacts by ID.</summary>
    Task<UseSendResponse<BulkDeleteResult>> BulkDeleteAsync(string contactBookId, BulkDeleteData data,
        CancellationToken cancellationToken = default);
}

internal sealed class ContactService : IContactService
{
    private readonly HttpExecutor _x;

    internal ContactService(HttpExecutor executor)
    {
        _x = executor;
    }


    /// <inheritdoc />
    public Task<UseSendResponse<string>> CreateAsync(string contactBookId, ContactData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"v1/contactBooks/{contactBookId}/contacts");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<ContactIdPayload, string>(req, x => x.ContactId!, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Contact>> GetAsync(string contactBookId, string contactId,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"v1/contactBooks/{contactBookId}/contacts/{contactId}");

        return _x.Execute<Contact, Contact>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<Contact>>> ListAsync(string contactBookId, ContactListQuery? query = null,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = $"v1/contactBooks/{contactBookId}/contacts";
        var url = baseUrl;

        if (query != null)
        {
            var qs = new Dictionary<string, string?>();

            if (query.Emails != null)
                qs.Add("emails", query.Emails);

            if (query.Ids != null)
                qs.Add("ids", query.Ids);

            if (query.Page.HasValue)
                qs.Add("page", query.Page.Value.ToString());

            if (query.Limit.HasValue)
                qs.Add("limit", query.Limit.Value.ToString());

            url = QueryHelpers.AddQueryString(baseUrl, qs);
        }

        var req = new HttpRequestMessage(HttpMethod.Get, url);

        return _x.Execute<List<Contact>, List<Contact>>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> UpdateAsync(string contactBookId, string contactId, ContactUpdateData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Patch, $"v1/contactBooks/{contactBookId}/contacts/{contactId}");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute(req, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<string>> UpsertAsync(string contactBookId, ContactData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Put,
            $"v1/contactBooks/{contactBookId}/contacts/{Uri.EscapeDataString(data.Email)}");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<ContactIdPayload, string>(req, x => x.ContactId!, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> DeleteAsync(string contactBookId, string contactId,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"v1/contactBooks/{contactBookId}/contacts/{contactId}");

        return _x.Execute(req, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<BulkCreateResult>> BulkCreateAsync(string contactBookId,
        IEnumerable<ContactData> contacts, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"v1/contactBooks/{contactBookId}/contacts/bulk");
        req.Content = JsonContent.Create(contacts, options: HttpExecutor.JsonOptions);

        return _x.Execute<BulkCreateResult, BulkCreateResult>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<BulkDeleteResult>> BulkDeleteAsync(string contactBookId, BulkDeleteData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"v1/contactBooks/{contactBookId}/contacts/bulk");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<BulkDeleteResult, BulkDeleteResult>(req, x => x, cancellationToken);
    }
}