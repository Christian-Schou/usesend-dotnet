using System.Net.Http.Json;

namespace UseSend;

/// <summary>
///     Operations on the <c>/v1/contactBooks</c> resource.
/// </summary>
public interface IContactBookService
{
    /// <summary>Lists all contact books accessible by the API key.</summary>
    Task<UseSendResponse<List<ContactBook>>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Creates a new contact book.</summary>
    Task<UseSendResponse<ContactBook>> CreateAsync(ContactBookData data, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a contact book by ID.</summary>
    Task<UseSendResponse<ContactBook>> GetAsync(string contactBookId, CancellationToken cancellationToken = default);

    /// <summary>Updates a contact book.</summary>
    Task<UseSendResponse> UpdateAsync(string contactBookId, ContactBookUpdateData data,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes a contact book.</summary>
    Task<UseSendResponse> DeleteAsync(string contactBookId, CancellationToken cancellationToken = default);
}

internal sealed class ContactBookService : IContactBookService
{
    private readonly HttpExecutor _x;

    internal ContactBookService(HttpExecutor executor)
    {
        _x = executor;
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<ContactBook>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "v1/contactBooks");

        return _x.Execute<List<ContactBook>, List<ContactBook>>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<ContactBook>> CreateAsync(ContactBookData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/contactBooks");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<ContactBook, ContactBook>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<ContactBook>> GetAsync(string contactBookId,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"v1/contactBooks/{contactBookId}");

        return _x.Execute<ContactBook, ContactBook>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> UpdateAsync(string contactBookId, ContactBookUpdateData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Patch, $"v1/contactBooks/{contactBookId}");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute(req, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> DeleteAsync(string contactBookId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"v1/contactBooks/{contactBookId}");

        return _x.Execute(req, cancellationToken);
    }
}