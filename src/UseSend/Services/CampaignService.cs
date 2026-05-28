using System.Net.Http.Json;

namespace UseSend;

/// <summary>
///     Operations on the <c>/v1/campaigns</c> resource.
/// </summary>
public interface ICampaignService
{
    /// <summary>Creates a new campaign.</summary>
    Task<UseSendResponse<Campaign>> CreateAsync(CampaignCreateData data, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a campaign by ID.</summary>
    Task<UseSendResponse<Campaign>> GetAsync(string campaignId, CancellationToken cancellationToken = default);

    /// <summary>Lists all campaigns.</summary>
    Task<UseSendResponse<List<Campaign>>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Deletes a campaign.</summary>
    Task<UseSendResponse> DeleteAsync(string campaignId, CancellationToken cancellationToken = default);

    /// <summary>Schedules (or immediately sends) a campaign.</summary>
    Task<UseSendResponse<Campaign>> ScheduleAsync(string campaignId, CampaignScheduleData data,
        CancellationToken cancellationToken = default);

    /// <summary>Pauses a sending campaign.</summary>
    Task<UseSendResponse<Campaign>> PauseAsync(string campaignId, CancellationToken cancellationToken = default);

    /// <summary>Resumes a paused campaign.</summary>
    Task<UseSendResponse<Campaign>> ResumeAsync(string campaignId, CancellationToken cancellationToken = default);
}

internal sealed class CampaignService : ICampaignService
{
    private readonly HttpExecutor _x;

    internal CampaignService(HttpExecutor executor)
    {
        _x = executor;
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Campaign>> CreateAsync(CampaignCreateData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "v1/campaigns");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<Campaign, Campaign>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Campaign>> GetAsync(string campaignId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"v1/campaigns/{campaignId}");

        return _x.Execute<Campaign, Campaign>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<List<Campaign>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, "v1/campaigns");

        return _x.Execute<List<Campaign>, List<Campaign>>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse> DeleteAsync(string campaignId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"v1/campaigns/{campaignId}");

        return _x.Execute(req, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Campaign>> ScheduleAsync(string campaignId, CampaignScheduleData data,
        CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"v1/campaigns/{campaignId}/schedule");
        req.Content = JsonContent.Create(data, options: HttpExecutor.JsonOptions);

        return _x.Execute<Campaign, Campaign>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Campaign>> PauseAsync(string campaignId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"v1/campaigns/{campaignId}/pause");

        return _x.Execute<Campaign, Campaign>(req, x => x, cancellationToken);
    }


    /// <inheritdoc />
    public Task<UseSendResponse<Campaign>> ResumeAsync(string campaignId, CancellationToken cancellationToken = default)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"v1/campaigns/{campaignId}/resume");

        return _x.Execute<Campaign, Campaign>(req, x => x, cancellationToken);
    }
}