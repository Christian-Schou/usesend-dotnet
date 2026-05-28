using System.Net.Http.Json;

namespace UseSend;

/// <summary>
///     Operations on the <c>/v1/domains</c> resource.
/// </summary>
public interface IDomainService
{
    /// <summary>Lists all domains accessible by the API key.</summary>
    Task<UseSendResponse<List<Domain>>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Creates a new domain.</summary>
    Task<UseSendResponse<Domain>> CreateAsync(DomainCreateData data, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a domain by ID.</summary>
    Task<UseSendResponse<Domain>> GetAsync(long domainId, CancellationToken cancellationToken = default);

    /// <summary>Deletes a domain.</summary>
    Task<UseSendResponse<DomainDeleteResult>> DeleteAsync(long domainId, CancellationToken cancellationToken = default);

    /// <summary>Triggers DNS verification for a domain.</summary>
    Task<UseSendResponse> VerifyAsync(long domainId, CancellationToken cancellationToken = default);
}

internal sealed class DomainService : IDomainService
{
    private readonly HttpExecutor _x;

    internal DomainService(HttpExecutor executor)
    {
        _x = executor;
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<Domain>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "v1/domains");

        return _x.Execute<List<Domain>, List<Domain>>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Domain>> CreateAsync(DomainCreateData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/domains");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<Domain, Domain>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Domain>> GetAsync(long domainId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"v1/domains/{domainId}");

        return _x.Execute<Domain, Domain>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<DomainDeleteResult>> DeleteAsync(long domainId,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"v1/domains/{domainId}");

        return _x.Execute<DomainDeleteResult, DomainDeleteResult>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> VerifyAsync(long domainId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Put, $"v1/domains/{domainId}/verify");

        return _x.Execute(req, cancellationToken);
    }
}